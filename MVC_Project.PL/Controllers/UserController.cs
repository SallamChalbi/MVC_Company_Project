using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Helpers;
using MVC_Project.PL.ViewModels;
using System.Buffers;
using System.Threading.Tasks;

namespace MVC_Project.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
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

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            return View(viewName, _mapper.Map<ApplicationUser, UserViewModel>(user));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            //ViewBag.Departments = departments;
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if(user is not null)
                    {
                        user.FName = userVM.FName;
                        user.LName = userVM.LName;
                        user.PhoneNumber = userVM.PhoneNumber;

                        await _userManager.UpdateAsync(user);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                    await _userManager.DeleteAsync(user);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(userVM);
        }
    }
}
