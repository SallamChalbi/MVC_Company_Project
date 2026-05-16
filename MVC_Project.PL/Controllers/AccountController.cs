using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Helpers;
using MVC_Project.PL.ViewModels;
using System.Threading.Tasks;

namespace MVC_Project.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
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
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            { 
                var user = await _userManager.FindByEmailAsync(model.Email!);

                if (user is not null)
                    if (await _userManager.CheckPasswordAsync(user, model.Password!))
                    { 
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password!, model.RememberMe, false);

                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                
                ModelState.AddModelError(string.Empty, "Incorrect Email or Password");
            }
            return View(model);
        }

        public IActionResult ForgotPassword()
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userManager.FindByEmailAsync(model.Email!);
                if (user is not null) 
                {
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = Token }, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        Body = passwordResetLink,
                        Recipient = model.Email
                    };
                    //EmailSettings.SendEmail(email);
                    _emailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Email is not Valid!");
            }
            return View(model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = (TempData["email"] as string)!;
                string token = (TempData["token"] as string)!;

                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user!, token, model.NewPassword!);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));

                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
