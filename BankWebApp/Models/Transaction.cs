using System;

namespace YourProjectName.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class Transaction
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? RelatedAccountNumber { get; set; }
    }
}
