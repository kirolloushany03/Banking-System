using Banking_System.Entites;

namespace Banking_System.Dtos.TransactionDtos
{
    public class CreateTransactionDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }         
        public TransactionType TransactionType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
