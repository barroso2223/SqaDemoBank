namespace SqaDemoBank.Models
{
    public class Transfer
    {
        public int Id { get; set; }

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // navigation (optional but nice)
        public Account? FromAccount { get; set; }
        public Account? ToAccount { get; set; }
    }
}
