using Microsoft.AspNetCore.Mvc;

namespace Echomedproject.PL.Controllers
{
    public class DataEntryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
