namespace YourProjectName.Models
{
    public class Account
    {
        public int Id { get; set; }                     // Unique account ID
        public string Name { get; set; } = string.Empty; // Owner name
        public decimal Balance { get; set; } = 0;       // Account balance
    }
}
