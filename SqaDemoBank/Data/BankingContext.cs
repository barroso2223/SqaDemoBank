using Microsoft.EntityFrameworkCore;
using SqaDemoBank.Models;

namespace SqaDemoBank.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transfer> Transfers => Set<Transfer>();
    }
}
