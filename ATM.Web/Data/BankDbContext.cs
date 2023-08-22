using ATM.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM.Web.Data
{
    public class BankDbContext: DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        public BankDbContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<BankDetail> BankDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LogDetail> LogDetails { get; set; }
    }
}
