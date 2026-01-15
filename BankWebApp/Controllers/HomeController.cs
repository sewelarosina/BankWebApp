using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;

namespace YourProjectName.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            });
        }
    }
}
