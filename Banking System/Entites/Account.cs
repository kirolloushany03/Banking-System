using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Entites
{
    [Index(nameof(AccountNumber), IsUnique = true)]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = null!;

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OverdraftLimit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestRate { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
