using AutoMapper;
using Banking_System.Data;
using Banking_System.Dtos.TransactionDtos;
using Banking_System.Entites;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public TransactionService(DataContext context, IMapper mapper, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
        }



        //public async Task<TransactionDto> DepositAsync(CreateDepositDto createDepositDto)
        //{
        //    // Update balance using the new method
        //    var account = await _accountService.UpdateAccountBalanceAsync(
        //        createDepositDto.AccountNumber,
        //        createDepositDto.Amount,
        //        isdeposit: true
        //    );

        //    // Create transaction
        //    var transaction = new Transaction
        //    {
        //        AccountId = account.Id,
        //        Amount = createDepositDto.Amount,
        //        TransactionType = TransactionType.Deposit,
        //        Timestamp = DateTime.UtcNow
        //    };

        //    // Log the transaction
        //    return _mapper.Map<TransactionDto>(await LogTransactionAsync(transaction));
        //}

        public async Task<TransactionDto> DepositAsync(CreateDepositDto createDepositDto)
        {
            // 1. Proactively find the account by its public number.
            var account = await _context.TbAccount
                .FirstOrDefaultAsync(a => a.AccountNumber == createDepositDto.AccountNumber);

            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            // 2. Update the balance.
            account.Balance += createDepositDto.Amount;

            // 3. Create the new Transaction, linking the full Account object.
            var transaction = new Transaction
            {
                Account = account, // Link the full object
                Amount = createDepositDto.Amount,
                TransactionType = TransactionType.Deposit,
                Timestamp = DateTime.UtcNow
            };

            // 4. Add the new transaction to be tracked.
            _context.TbTransaction.Add(transaction);

            // 5. Save all changes (the balance update and the new transaction) at once.
            await _context.SaveChangesAsync();

            // 6. Map the complete transaction object to a DTO for the response.
            return _mapper.Map<TransactionDto>(transaction);
        }

        //public async Task<TransactionDto> WithdrawAsync(CreateWithdrawDto createWithdrawDto)
        //{
        //    // Update balance using the new method
        //    var account = await _accountService.UpdateAccountBalanceAsync(
        //        createWithdrawDto.AccountNumber,
        //        createWithdrawDto.Amount,
        //        isdeposit: false
        //    );

        //    // Create transaction
        //    var transaction = new Transaction
        //    {
        //        AccountId = account.Id,
        //        Amount = createWithdrawDto.Amount,
        //        TransactionType = TransactionType.Withdrawal,
        //        Timestamp = DateTime.UtcNow
        //    };

        //    // Log the transaction
        //    return _mapper.Map<TransactionDto>(await LogTransactionAsync(transaction));
        //}

        public async Task<TransactionDto> WithdrawAsync(CreateWithdrawDto createWithdrawDto)
        {
            // 1. Proactively find the account.
            var account = await _context.TbAccount
                .FirstOrDefaultAsync(a => a.AccountNumber == createWithdrawDto.AccountNumber);

            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            // 2. Perform validation checks before changing anything.
            if (account.AccountType == AccountType.checking && (account.Balance + account.OverdraftLimit) < createWithdrawDto.Amount)
            {
                throw new Exception("Insufficient funds, including overdraft limit.");
            }
            if (account.AccountType == AccountType.savings && account.Balance < createWithdrawDto.Amount)
            {
                throw new Exception("Insufficient funds in savings account.");
            }

            // 3. Update the balance.
            account.Balance -= createWithdrawDto.Amount;

            // 4. Create the new Transaction, linking the full Account object.
            var transaction = new Transaction
            {
                Account = account, // Link the full object
                Amount = createWithdrawDto.Amount,
                TransactionType = TransactionType.Withdrawal,
                Timestamp = DateTime.UtcNow
            };

            // 5. Add and Save all changes at once.
            _context.TbTransaction.Add(transaction);
            await _context.SaveChangesAsync();

            // 6. Map and return the complete DTO.
            return _mapper.Map<TransactionDto>(transaction);
        }

        /*public async Task<TransactionDto> TransferAsync(CreateTransferDto createTransferDto)
        {
            // Deduct balance from source account
            var sourceAccount = await _accountService.UpdateAccountBalanceAsync(
                createTransferDto.SourceAccountNumber,
                createTransferDto.Amount,
                isdeposit: false
            );

            // Add balance to target account
            var targetAccount = await _accountService.UpdateAccountBalanceAsync(
                createTransferDto.TargetAccountNumber,
                createTransferDto.Amount,
                isdeposit: true
            );

            // Create transaction for the source account
            var sourceTransaction = new Transaction
            {
                AccountId = sourceAccount.Id,
                Amount = createTransferDto.Amount,
                TransactionType = TransactionType.transfer,
                Timestamp = DateTime.UtcNow
            };

            // Log transaction for source account
            await LogTransactionAsync(sourceTransaction);

            // Create transaction for the target account
            var targetTransaction = new Transaction
            {
                AccountId = targetAccount.Id,
                Amount = createTransferDto.Amount,
                TransactionType = TransactionType.Deposit, // Transfer viewed as a deposit for target
                Timestamp = DateTime.UtcNow
            };

            // Log transaction for target account
            await LogTransactionAsync(targetTransaction);

            return _mapper.Map<TransactionDto>(sourceTransaction);
        }*/

        public async Task<TransactionDto> TransferAsync(CreateTransferDto createTransferDto)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var accountNumbers = new[] { createTransferDto.SourceAccountNumber, createTransferDto.TargetAccountNumber };
                var accounts = await _context.TbAccount
                    .Where(a => accountNumbers.Contains(a.AccountNumber))
                    .ToListAsync(); // Fetches a list of up to 2 accounts.

                var sourceAccount = accounts.FirstOrDefault(a => a.AccountNumber == createTransferDto.SourceAccountNumber);
                var targetAccount = accounts.FirstOrDefault(a => a.AccountNumber == createTransferDto.TargetAccountNumber);

                if (sourceAccount == null)
                {
                    throw new Exception("Source account not found.");
                }

                if (sourceAccount.AccountType == AccountType.savings && sourceAccount.Balance < createTransferDto.Amount)
                {
                    throw new Exception("Insufficient funds in savings account.");
                }

                if (sourceAccount.AccountType == AccountType.checking && (sourceAccount.Balance + sourceAccount.OverdraftLimit) < createTransferDto.Amount)
                {
                    throw new Exception("Insufficient funds, including overdraft limit.");
                }

                

                if (targetAccount == null)
                {
                    throw new Exception("Target account not found.");
                }

                sourceAccount.Balance -= createTransferDto.Amount;
                targetAccount.Balance += createTransferDto.Amount;

                var sourceTransaction = new Transaction
                {
                    AccountId = sourceAccount.Id,
                    Amount = createTransferDto.Amount,
                    TransactionType = TransactionType.transfer, // This is an outgoing transfer
                    Timestamp = DateTime.UtcNow
                };

                var targetTransaction = new Transaction
                {
                    AccountId = targetAccount.Id,
                    Amount = createTransferDto.Amount,
                    TransactionType = TransactionType.Deposit, // For the target, a transfer is a deposit
                    Timestamp = DateTime.UtcNow
                };

                _context.TbTransaction.Add(sourceTransaction);
                _context.TbTransaction.Add(targetTransaction);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return _mapper.Map<TransactionDto>(sourceTransaction);
            }
            catch (Exception ex) 
            {
                await dbTransaction.RollbackAsync();

                throw;
            }
        }

        //public async Task<TransactionDto> LogTransactionAsync(Transaction transaction)
        //{
        //    // Add transaction to database
        //    _context.TbTransaction.Add(transaction);
        //    await _context.SaveChangesAsync();
        //    return _mapper.Map<TransactionDto>(transaction);
        //}

        public async Task<CreateTransactionResultDto> CreateManualTransactionAsync(CreateTransactionDto dto)
        { 
            var account  = await _context.TbAccount.FindAsync(dto.AccountId);
            if (account == null)
            { 
                throw new Exception($"Account id {dto.AccountId} not found");
            }

            var newTransaction = new Transaction
            {
                Account = account,
                Amount = dto.Amount,
                TransactionType = dto.TransactionType,
                Timestamp = DateTime.UtcNow
            };

            _context.TbTransaction.Add(newTransaction);
            await _context.SaveChangesAsync();

            return new CreateTransactionResultDto
            {
                Id = newTransaction.Id,
                AccountNumber = newTransaction.Account.AccountNumber,
                Amount = newTransaction.Amount,
                TransactionType = newTransaction.TransactionType,
                Timestamp = newTransaction.Timestamp
            };
        }
    }
}
