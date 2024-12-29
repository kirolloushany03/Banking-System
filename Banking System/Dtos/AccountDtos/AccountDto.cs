using Banking_System.Entites;

namespace Banking_System.Dtos.AccountDtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public decimal? OverdraftLimit { get; set; }
        public decimal? InterestRate { get; set; }
    }
}
