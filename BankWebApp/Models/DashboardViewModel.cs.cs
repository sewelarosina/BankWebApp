using System.Collections.Generic;

namespace YourProjectName.Models
{
    public class DashboardViewModel
    {
        public Account LoggedIn { get; set; } = new Account();   // The currently active account
        public List<Account> AllAccounts { get; set; } = new List<Account>(); // All accounts in the system
    }
}
