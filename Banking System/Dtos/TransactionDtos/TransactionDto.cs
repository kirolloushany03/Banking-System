using Banking_System.Entites;

namespace Banking_System.Dtos.TransactionDtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
