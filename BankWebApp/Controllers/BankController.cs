using Microsoft.AspNetCore.Mvc;
using BankWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankWebApp.Controllers
{
    public class BankController : Controller
    {
        private static List<BankAccount> accounts = new();
        private static List<Transaction> transactions = new();
        private static int nextAccountNumber = 1001;
        private static int nextTransactionId = 1;

        // ---------------------- DASHBOARD ----------------------
        public IActionResult Index()
        {
            return View(accounts);
        }

        // ---------------------- CREATE ACCOUNT ----------------------
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(string owner, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                TempData["Error"] = "Owner name cannot be empty.";
                return RedirectToAction("Create");
            }

            accounts.Add(new BankAccount
            {
                AccountNumber = nextAccountNumber++,
                Owner = owner,
                Balance = initialBalance
            });

            TempData["Success"] = $"Account created successfully for {owner}!";
            return RedirectToAction("Index");
        }

        // ---------------------- DEPOSIT ----------------------
        public IActionResult Deposit(int id)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            return View(acc);
        }

        [HttpPost]
        public IActionResult Deposit(int id, decimal amount)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            if (amount <= 0) { TempData["Error"] = "Deposit must be positive."; return RedirectToAction("Deposit", new { id }); }

            acc.Balance += amount;

            transactions.Add(new Transaction
            {
                TransactionId = nextTransactionId++,
                AccountNumber = id,
                Type = "Deposit",
                Amount = amount,
                Description = $"Deposited ${amount}",
                Date = System.DateTime.Now
            });

            TempData["Success"] = $"Successfully deposited ${amount} to account {id}.";
            return RedirectToAction("Index");
        }

        // ---------------------- WITHDRAW ----------------------
        public IActionResult Withdraw(int id)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            return View(acc);
        }

        [HttpPost]
        public IActionResult Withdraw(int id, decimal amount)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            if (amount <= 0) { TempData["Error"] = "Withdrawal must be positive."; return RedirectToAction("Withdraw", new { id }); }
            if (amount > acc.Balance) { TempData["Error"] = "Insufficient balance!"; return RedirectToAction("Withdraw", new { id }); }

            acc.Balance -= amount;

            transactions.Add(new Transaction
            {
                TransactionId = nextTransactionId++,
                AccountNumber = id,
                Type = "Withdraw",
                Amount = amount,
                Description = $"Withdrew ${amount}",
                Date = System.DateTime.Now
            });

            TempData["Success"] = $"Successfully withdrew ${amount} from account {id}.";
            return RedirectToAction("Index");
        }

        // ---------------------- EDIT ----------------------
        public IActionResult Edit(int id)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            return View(acc);
        }

        [HttpPost]
        public IActionResult Edit(int id, string owner, decimal balance)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            if (string.IsNullOrWhiteSpace(owner)) { TempData["Error"] = "Owner cannot be empty."; return RedirectToAction("Edit", new { id }); }

            acc.Owner = owner;
            acc.Balance = balance;

            TempData["Success"] = $"Account {id} updated successfully!";
            return RedirectToAction("Index");
        }

        // ---------------------- DELETE ----------------------
        public IActionResult Delete(int id)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc == null) { TempData["Error"] = "Account not found."; return RedirectToAction("Index"); }
            return View(acc);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var acc = accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (acc != null)
            {
                accounts.Remove(acc);
                TempData["Success"] = $"Account {id} deleted successfully!";
            }
            else TempData["Error"] = "Account not found.";
            return RedirectToAction("Index");
        }

        // ---------------------- TRANSFER ----------------------
        public IActionResult Transfer() => View();

        [HttpPost]
        public IActionResult Transfer(int from, int to, decimal amount)
        {
            var sender = accounts.FirstOrDefault(a => a.AccountNumber == from);
            var receiver = accounts.FirstOrDefault(a => a.AccountNumber == to);

            if (amount <= 0)
            {
                TempData["Error"] = "Transfer amount must be positive.";
                return RedirectToAction("Transfer");
            }

            // Deduct from sender if exists
            if (sender != null)
            {
                if (sender.Balance < amount)
                {
                    TempData["Error"] = "Sender has insufficient balance.";
                    return RedirectToAction("Transfer");
                }

                sender.Balance -= amount;

                transactions.Add(new Transaction
                {
                    TransactionId = nextTransactionId++,
                    AccountNumber = from,
                    Type = "Transfer Out",
                    Amount = amount,
                    Description = $"Transferred ${amount} to account {to}",
                    Date = System.DateTime.Now
                });
            }
            else
            {
                // Sender doesn't exist, just log
                transactions.Add(new Transaction
                {
                    TransactionId = nextTransactionId++,
                    AccountNumber = from,
                    Type = "Transfer Out",
                    Amount = amount,
                    Description = $"Transferred ${amount} to account {to} (nonexistent sender)",
                    Date = System.DateTime.Now
                });
            }

            // Add to receiver if exists
            if (receiver != null)
            {
                receiver.Balance += amount;

                transactions.Add(new Transaction
                {
                    TransactionId = nextTransactionId++,
                    AccountNumber = to,
                    Type = "Transfer In",
                    Amount = amount,
                    Description = $"Received ${amount} from account {from}",
                    Date = System.DateTime.Now
                });
            }
            else
            {
                // Receiver doesn't exist, just log
                transactions.Add(new Transaction
                {
                    TransactionId = nextTransactionId++,
                    AccountNumber = to,
                    Type = "Transfer In",
                    Amount = amount,
                    Description = $"Received ${amount} from account {from} (nonexistent receiver)",
                    Date = System.DateTime.Now
                });
            }

            TempData["Success"] = $"Successfully transferred ${amount} from {from} to {to}.";
            return RedirectToAction("Index");
        }

        // ---------------------- TRANSACTIONS HISTORY ----------------------
        public IActionResult Transactions()
        {
            var sorted = transactions.OrderByDescending(t => t.Date).ToList();
            return View(sorted);
        }
    }
}
