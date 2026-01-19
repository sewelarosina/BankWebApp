using System;

namespace BankWebApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public string Type { get; set; } = string.Empty; // Deposit, Withdraw, Transfer In/Out
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
