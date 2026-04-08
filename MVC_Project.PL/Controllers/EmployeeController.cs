using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Models;
using MVC_Project.PL.ViewModels;

namespace MVC_Project.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IEnumerable<Department> departments;

        public EmployeeController(
            //IEmployeeRepository repository, 
            //IDepartmentRepository departmentRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_employeeRepository = repository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //departments = _departmentRepository.GetAll();
        }
        public IActionResult Index(string SearchValue)
        {
            ///// Data Binding 
            //// 1. KeyValuePair => Dictionary object 
            //ViewData["Message"] = "Viw Data";
            //// 2. Dynamic Property => Dynamic keyword 
            //ViewBag.Message = "View Bag";
            //// => 1 & 2: Transfer Data from Action to it's View / from View to _Layout 

            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                //employees = _employeeRepository.GetAll();
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                //employees = _employeeRepository.GetEmployeesByName(SearchValue);
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            var reverseMappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            //or _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            ViewBag.InputValue = SearchValue;
            return View(reverseMappedEmployee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            //ViewBag.Departments = departments;
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                //or _mapper.Map<Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                if (_unitOfWork.Complete() > 0)
                    // 3. KeyValuePair => Dictionary object 
                    // Transfer Data from Action to Action 
                    TempData["Message"] = "Employee Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.Departments = departments;
            return View(employeeVM);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();
            return View(viewName, _mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = departments;
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            //ViewBag.Departments = departments;
            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(employeeVM);
        }
    }
}
