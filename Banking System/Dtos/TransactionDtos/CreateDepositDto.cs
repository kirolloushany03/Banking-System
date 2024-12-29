namespace Banking_System.Dtos.TransactionDtos
{
    public class CreateDepositDto
    {
        public string AccountNumber { get; set; } = null!;

        public decimal Amount { get; set; }
    }
}
