using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Models;
using MVC_Project.PL.Helpers;
using MVC_Project.PL.ViewModels;
using System.Threading.Tasks;

namespace MVC_Project.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string SearchValue)
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
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (employeeVM.Image is not null)
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                //or _mapper.Map<Employee>(employeeVM);
                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                if (await _unitOfWork.CompleteAsync() > 0)
                    // 3. KeyValuePair => Dictionary object 
                    // Transfer Data from Action to Action 
                    TempData["Message"] = "Employee Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.Departments = departments;
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            return View(viewName, _mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = departments;
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVM.Image is not null)
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                if(!string.IsNullOrEmpty(mappedEmployee.ImageName))
                    DocumentSettings.DeleteFile(mappedEmployee.ImageName, "Images");
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                await _unitOfWork.CompleteAsync();
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
