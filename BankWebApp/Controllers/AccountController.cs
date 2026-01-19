using Microsoft.AspNetCore.Mvc;

namespace YourAppName.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login (ALLOW ANY CREDENTIALS)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            HttpContext.Session.SetString("Username", username);
            return RedirectToAction("Index", "Bank");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
