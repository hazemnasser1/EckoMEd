using Echomedproject.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Echomedproject.PL.Controllers
{
    public class UserController : Controller
    {
        private IPatientRepository patientRepository;

        public UserController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
