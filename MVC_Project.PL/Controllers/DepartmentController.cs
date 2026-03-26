using Microsoft.AspNetCore.Mvc;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Models;

namespace MVC_Project.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var Department = _repository.GetAll();
            return View(Department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid)
            {
                _repository.Add(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _repository.GetById(id.Value);
            if(department is null)
                return NotFound();
            return View(viewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Edit([FromRoute]int id, Department department)
        {
            if(id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    throw;
                }
            }
            return View(department);
        }
    }
}
