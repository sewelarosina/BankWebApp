using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using YourProjectName.Models;

namespace YourProjectName.Controllers
{
    public class BankController : Controller
    {
        private static List<BankAccount> accounts = new();
        private static List<Transaction> transactions = new();
        private static int transactionIdCounter = 1;

        public IActionResult Index()
        {
            return View(accounts);
        }

        // Create Account GET
        public IActionResult Create()
        {
            return View();
        }

        // Create Account POST
        [HttpPost]
        public IActionResult Create(BankAccount model)
        {
            if (ModelState.IsValid)
            {
                accounts.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Transfer GET
        public IActionResult Transfer()
        {
            return View();
        }

        // Transfer POST
        [HttpPost]
        public IActionResult Transfer(TransferViewModel model)
        {
            var from = accounts.FirstOrDefault(a => a.AccountNumber == model.FromAccount);
            var to = accounts.FirstOrDefault(a => a.AccountNumber == model.ToAccount);

            if (from == null || to == null || model.Amount <= 0 || from.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Invalid transfer.");
                return View(model);
            }

            from.Balance -= model.Amount;
            to.Balance += model.Amount;

            transactions.Add(new Transaction
            {
                Id = transactionIdCounter++,
                AccountNumber = from.AccountNumber,
                Type = TransactionType.Transfer,
                Amount = model.Amount,
                Date = DateTime.Now,
                RelatedAccountNumber = to.AccountNumber
            });

            transactions.Add(new Transaction
            {
                Id = transactionIdCounter++,
                AccountNumber = to.AccountNumber,
                Type = TransactionType.Transfer,
                Amount = model.Amount,
                Date = DateTime.Now,
                RelatedAccountNumber = from.AccountNumber
            });

            return RedirectToAction("Index");
        }

        // Deposit GET
        public IActionResult Deposit(int accountNumber)
        {
            var account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound();

            return View(new AmountViewModel { AccountNumber = accountNumber });
        }

        // Deposit POST
        [HttpPost]
        public IActionResult Deposit(AmountViewModel model)
        {
            var account = accounts.FirstOrDefault(a => a.AccountNumber == model.AccountNumber);

            if (account == null || model.Amount <= 0)
            {
                ModelState.AddModelError("", "Invalid deposit.");
                return View(model);
            }

            account.Balance += model.Amount;

            transactions.Add(new Transaction
            {
                Id = transactionIdCounter++,
                AccountNumber = account.AccountNumber,
                Type = TransactionType.Deposit,
                Amount = model.Amount,
                Date = DateTime.Now
            });

            return RedirectToAction("Index");
        }

        // Withdraw GET
        public IActionResult Withdraw(int accountNumber)
        {
            var account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound();

            return View(new AmountViewModel { AccountNumber = accountNumber });
        }

        // Withdraw POST
        [HttpPost]
        public IActionResult Withdraw(AmountViewModel model)
        {
            var account = accounts.FirstOrDefault(a => a.AccountNumber == model.AccountNumber);

            if (account == null || model.Amount <= 0 || account.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Invalid withdrawal.");
                return View(model);
            }

            account.Balance -= model.Amount;

            transactions.Add(new Transaction
            {
                Id = transactionIdCounter++,
                AccountNumber = account.AccountNumber,
                Type = TransactionType.Withdraw,
                Amount = model.Amount,
                Date = DateTime.Now
            });

            return RedirectToAction("Index");
        }

        // Transaction History
        public IActionResult Transactions(int accountNumber)
        {
            var account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                return NotFound();
            }

            var accountTransactions = transactions
                .Where(t => t.AccountNumber == accountNumber)
                .OrderByDescending(t => t.Date)
                .ToList();

            ViewBag.Account = account;
            return View(accountTransactions);
        }
    }
}
