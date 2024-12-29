using Banking_System.Entites;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Account> TbAccount { get; set; } = null!;
        public DbSet<Transaction> TbTransaction { get; set; } = null!;

        //adding the realtionship manually one-to-many relationship
        //with ondelete transactions if we 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

        }
    }
}
