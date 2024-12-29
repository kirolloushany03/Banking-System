using Banking_System.Dtos.AccountDtos;
using Banking_System.Entites;

namespace Banking_System.Services
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto);
        Task<decimal> GetBalance(string AccountNumber);
        Task<AccountDto> UpdateAccountBalanceAsync(string AccountNumber, decimal Amount, bool isdeposit);
        Task CalculateMonthlyInterest();

    }
}
