namespace BankWebApp.Models
{
    public class BankAccount
    {
        public int AccountNumber { get; set; }
        public string Owner { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
