using Microsoft.EntityFrameworkCore;
using SubTracker.capstone_Kellogg.Models;

namespace SubTracker.capstone_Kellogg.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
    : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Autopayment> Autopayments { get; set; }
    }
}
