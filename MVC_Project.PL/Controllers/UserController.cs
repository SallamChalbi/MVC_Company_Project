using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Project.DAL.Models;
using MVC_Project.PL.ViewModels;
using System.Buffers;
using System.Threading.Tasks;

namespace MVC_Project.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                //var users = await _userManager.Users.Select(U => new UserViewModel()
                //{
                //    Id = U.Id,
                //    FName = U.FName,
                //    LName = U.LName,
                //    Email = U.Email,
                //    PhoneNumber = U.PhoneNumber,
                //    Roles = _userManager.GetRolesAsync(U).Result
                //}).ToListAsync();
                var users = await _userManager.Users.AsNoTracking().ToListAsync();

                var userViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    userViewModels.Add(new UserViewModel
                    {
                        Id = user.Id,
                        FName = user.FName,
                        LName = user.LName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = roles
                    });
                }
                ViewBag.InputValue = email;
                return View(userViewModels);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                var mappedUser = new UserViewModel()
                {
                    Id = user?.Id,
                    FName = user?.FName,
                    LName = user?.LName,
                    Email = user?.Email,
                    PhoneNumber = user?.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user!).Result
                };
                ViewBag.InputValue = email;
                return View(new List<UserViewModel> () { mappedUser });
            }
        }
    }
}
