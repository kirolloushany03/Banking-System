namespace Banking_System.Dtos.TransactionDtos
{
    public class CreateTransferDto
    {
        public String SourceAccountNumber { get; set; } = null!;
        public String TargetAccountNumber { get; set; } = null!;
         public decimal Amount { get; set; }
    }
}
