using Microsoft.AspNetCore.Mvc;

namespace MVC_Project.PL.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
