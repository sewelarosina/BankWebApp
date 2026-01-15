using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourProjectName.Controllers
{
    public class BankController : Controller
    {
        private static List<BankAccount> accounts = new();

        public IActionResult Index()
        {
            return View(accounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BankAccount account)
        {
            account.AccountNumber = accounts.Count + 1001;
            accounts.Add(account);
            return RedirectToAction("Index");
        }

        public IActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Transfer(TransferViewModel model)
        {
            var from = accounts.FirstOrDefault(a => a.AccountNumber == model.FromAccount);
            var to = accounts.FirstOrDefault(a => a.AccountNumber == model.ToAccount);

            if (from == null || to == null || from.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Invalid transfer details.");
                return View(model);
            }

            from.Balance -= model.Amount;
            to.Balance += model.Amount;

            return RedirectToAction("Index");
        }
    }
}
