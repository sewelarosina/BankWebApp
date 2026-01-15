using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BankWebApp.Controllers
{
    public class BankController : Controller
    {
        // In-memory storage
        private static List<BankAccount> Accounts = new List<BankAccount>();
        private static int NextAccountNumber = 1001;

        // List all accounts
        public IActionResult Index()
        {
            return View(Accounts);
        }

        // Create account page
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string owner)
        {
            if (!string.IsNullOrWhiteSpace(owner))
            {
                var account = new BankAccount { Owner = owner, AccountNumber = NextAccountNumber++ };
                Accounts.Add(account);
            }
            return RedirectToAction("Index");
        }

        // Deposit page
        public IActionResult Deposit(int id)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == id);
            return View(account);
        }

        [HttpPost]
        public IActionResult Deposit(int accountNumber, decimal amount)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null && amount > 0)
                account.Balance += amount;

            return RedirectToAction("Index");
        }

        // Withdraw page
        public IActionResult Withdraw(int id)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == id);
            return View(account);
        }

        [HttpPost]
        public IActionResult Withdraw(int accountNumber, decimal amount)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null && amount > 0 && amount <= account.Balance)
                account.Balance -= amount;

            return RedirectToAction("Index");
        }

        // Transfer page
        public IActionResult Transfer()
        {
            ViewBag.Accounts = Accounts;
            return View();
        }

        [HttpPost]
        public IActionResult Transfer(int fromAccount, int toAccount, decimal amount)
        {
            var sender = Accounts.FirstOrDefault(a => a.AccountNumber == fromAccount);
            var receiver = Accounts.FirstOrDefault(a => a.AccountNumber == toAccount);

            if (sender != null && receiver != null && amount > 0 && sender.Balance >= amount)
            {
                sender.Balance -= amount;
                receiver.Balance += amount;
            }

            return RedirectToAction("Index");
        }
    }
}
