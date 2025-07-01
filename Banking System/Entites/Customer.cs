using System.ComponentModel.DataAnnotations;

namespace Banking_System.Entites
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "Customer"; // Default role

        // A customer can have many accounts
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
