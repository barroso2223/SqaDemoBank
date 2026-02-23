using SqaDemoBank.Models;

namespace SqaDemoBank.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BankingContext context)
        {
            context.Database.EnsureCreated();

            if (context.Accounts.Any())
            {
                return; // DB already seeded
            }

            var accounts = new[]
            {
                new Account { OwnerName = "Alice", Balance = 1500m },
                new Account { OwnerName = "Bob", Balance = 800m }
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }
    }
}
