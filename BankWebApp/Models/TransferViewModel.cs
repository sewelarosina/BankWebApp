namespace BankWebApp.Models
{
    public class TransferViewModel
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public List<BankAccount> Accounts { get; set; } = new();
    }
}
