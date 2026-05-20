using Microsoft.EntityFrameworkCore;
using loyalty_application.Models;

namespace loyalty_application.Data
{
    public class RewardsDbContext : DbContext
    {
        public RewardsDbContext(DbContextOptions<RewardsDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
        public DbSet<PointLedgerEntry> PointLedgerEntries { get; set; }
    }
}