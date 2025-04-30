using Microsoft.AspNetCore.Mvc;

namespace Echomedproject.PL.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
