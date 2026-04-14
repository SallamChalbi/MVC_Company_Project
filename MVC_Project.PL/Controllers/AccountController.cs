using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.DAL.Models;
using MVC_Project.PL.ViewModels;
using System.Threading.Tasks;

namespace MVC_Project.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FName = model.FName,
                    LName = model.LName,
                    Email = model.Email,
                    UserName = model.Email?.Split('@')[0],
                    IsAgree = model.IsAgree,
                };
                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach(var error in result.Errors) 
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
