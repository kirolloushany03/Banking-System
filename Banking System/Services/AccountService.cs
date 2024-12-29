using System.Threading.Tasks;
using AutoMapper;
using Banking_System.Data;
using Banking_System.Dtos.AccountDtos;
using Banking_System.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using static Banking_System.Data.DataContext;

namespace Banking_System.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AccountService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Method to create an account
        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            // Check if an account with the same AccountNumber already exists
            var existingAccount = await _context.TbAccount
                .FirstOrDefaultAsync(a => a.AccountNumber == createAccountDto.AccountNumber);

            if (existingAccount != null)
            {
                // Handle the case where the account number already exists
                throw new ArgumentException("Account number already exists.");
            }

            // Map the CreateAccountDto to an Account entity
            var account = _mapper.Map<Account>(createAccountDto);

            // Add the account to the context
            _context.TbAccount.Add(account);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the Account entity to an AccountDto and return it
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<decimal> GetBalance(string AccountNumber) 
        {
            var account = await _context.TbAccount.FirstOrDefaultAsync(a => a.AccountNumber == AccountNumber);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            return account.Balance;
        }

        public async Task<AccountDto> UpdateAccountBalanceAsync(string AccountNumber, decimal Amount, bool isdeposit)
        { 
            var account  = await _context.TbAccount.FirstOrDefaultAsync(a => a.AccountNumber == AccountNumber);

            if (account == null)
                throw new Exception("Account not found");

            if (isdeposit)
            {
                account.Balance += Amount;
            }

            else
            {
                if (account.AccountType == AccountType.checking && (account.Balance + account.OverdraftLimit) >= Amount)
                {
                    account.Balance -= Amount;
                }
                else if (account.AccountType == AccountType.savings && account.Balance >= Amount)
                {
                    account.Balance -= Amount;
                }
                else
                {
                    throw new Exception("Insufficient funds");
                }
            }

            _context.TbAccount.Update(account);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }


        // this the calculate monthly interest will be add automatically
        // every first day of the month at midnight using CronScheduler
        // but this of course has vulnerable so we can not depend on it
        public async Task CalculateMonthlyInterest()
        {
            var accounts = _context.TbAccount.Where(a => a.AccountType == AccountType.savings).ToList();

            foreach (var account in accounts)
            {
                decimal currentbalance = account.Balance;
                account.Balance  = currentbalance + (currentbalance * account.InterestRate ?? 0m); // Example for savings account
                _context.TbAccount.Update(account);
            }
            await _context.SaveChangesAsync();
        }



    }
}
