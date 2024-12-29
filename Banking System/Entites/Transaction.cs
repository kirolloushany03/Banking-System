using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking_System.Entites
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int AccountId { get; set; }

        [Required]
        public Account Account { get; set; } = null!;

        [Required]
        public TransactionType TransactionType { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
