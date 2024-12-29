using Banking_System.Entites;

namespace Banking_System.Dtos.AccountDtos
{
    public class CreateAccountDto
    {
        public string AccountNumber { get; set; } = null!;
        public AccountType AccountType { get; set; }
        public decimal InitialBalance { get; set; }

        public decimal? OverdraftLimit { get; set; }
        public decimal? InterestRate { get; set; }

    }
}
