using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Helpers;
using MVC_Project.PL.ViewModels;

namespace MVC_Project.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(U => new RoleViewModel()
                {
                    Id = U.Id,
                    RoleName = U.Name
                }).ToListAsync();

                ViewBag.InputValue = name;
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                var mappedRole = new RoleViewModel()
                {
                    Id = role?.Id,
                    RoleName = role?.Name,
                };
                ViewBag.InputValue = name;
                return View(new List<RoleViewModel>() { mappedRole });
            }
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(RoleViewModel roleVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
        //        await _roleManager.CreateAsync(mappedRole);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(roleVM);
        //}
    }
}
