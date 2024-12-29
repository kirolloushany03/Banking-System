using AutoMapper;
using Banking_System.Data;
using Banking_System.Dtos.TransactionDtos;
using Banking_System.Entites;

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



        public async Task<TransactionDto> DepositAsync(CreateDepositDto createDepositDto)
        {
            // Update balance using the new method
            var account = await _accountService.UpdateAccountBalanceAsync(
                createDepositDto.AccountNumber,
                createDepositDto.Amount,
                isdeposit: true
            );

            // Create transaction
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = createDepositDto.Amount,
                TransactionType = TransactionType.Deposit,
                Timestamp = DateTime.UtcNow
            };

            // Log the transaction
            return _mapper.Map<TransactionDto>(await LogTransactionAsync(transaction));
        }

        public async Task<TransactionDto> WithdrawAsync(CreateWithdrawDto createWithdrawDto)
        {
            // Update balance using the new method
            var account = await _accountService.UpdateAccountBalanceAsync(
                createWithdrawDto.AccountNumber,
                createWithdrawDto.Amount,
                isdeposit: false
            );

            // Create transaction
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = createWithdrawDto.Amount,
                TransactionType = TransactionType.Withdrawal,
                Timestamp = DateTime.UtcNow
            };

            // Log the transaction
            return _mapper.Map<TransactionDto>(await LogTransactionAsync(transaction));
        }

        public async Task<TransactionDto> TransferAsync(CreateTransferDto createTransferDto)
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

            return _mapper.Map<TransactionDto>(sourceTransaction); // Return the transaction related to the source account
        }

        public async Task<TransactionDto> LogTransactionAsync(Transaction transaction)
        {
            // Add transaction to database
            _context.TbTransaction.Add(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
