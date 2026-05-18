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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            return View(viewName, _mapper.Map<IdentityRole, RoleViewModel>(role));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            //ViewBag.Departments = departments;
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role is not null)
                    {
                        role.Name = roleVM.RoleName;

                        await _roleManager.UpdateAsync(role);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is not null)
                    await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(roleVM);
        }
    }
}
