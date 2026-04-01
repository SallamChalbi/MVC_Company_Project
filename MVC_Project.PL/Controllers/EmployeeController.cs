using Microsoft.AspNetCore.Mvc;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Models;

namespace MVC_Project.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEnumerable<Department> departments;

        public EmployeeController(IEmployeeRepository repository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = repository;
            _departmentRepository = departmentRepository;
            departments = _departmentRepository.GetAll();
        }
        public IActionResult Index()
        {
            /// Data Binding 
            // 1. KeyValuePair => Dictionary object 
            ViewData["Message"] = "Viw Data";
            // 2. Dynamic Property => Dynamic keyword 
            ViewBag.Message = "View Bag";
            // => 1 & 2: Transfer Data from Action to it's View / from View to _Layout 
            var Employees = _employeeRepository.GetAll();
            return View(Employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            ViewBag.Departments = departments;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (_employeeRepository.Add(employee) > 0)
                    // 3. KeyValuePair => Dictionary object 
                    // Transfer Data from Action to Action 
                    TempData["Message"] = "Employee Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = departments;
            return View(employee);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();
            
            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.Departments = departments;
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.Departments = departments;
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            try
            {
                _employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(employee);
        }
    }
}
