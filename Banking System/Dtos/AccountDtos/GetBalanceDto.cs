namespace Banking_System.Dtos.AccountDtos
{
    public class GetBalanceDto
    {
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
