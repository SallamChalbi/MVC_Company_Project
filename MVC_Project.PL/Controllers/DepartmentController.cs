using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Models;
using MVC_Project.PL.ViewModels;

namespace MVC_Project.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(
            //IDepartmentRepository repository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            /// Data Binding 
            // 1. KeyValuePair => Dictionary object 
            ViewData["Message"] = "Viw Data";
            // 2. Dynamic Property => Dynamic keyword 
            ViewBag.Message = "View Bag";
            // => 1 & 2: Transfer Data from Action to it's View / from View to _Layout 
            //var Departments = _repository.GetAll();
            var Departments = _unitOfWork.DepartmentRepository.GetAll();
            var reverseMappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(Departments);
                                     //or _mapper.Map<IEnumerable<DepartmentViewModel>>(Departments);
            return View(reverseMappedDepartment);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                // or _mapper.Map<Department>(departmentVM)
                _unitOfWork.DepartmentRepository.Add(mappedDepartment);
                if (_unitOfWork.Complete() > 0)
                    // 3. KeyValuePair => Dictionary object 
                    // Transfer Data from Action to Action 
                    TempData["Message"] = "Department Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if(department is null)
                return NotFound();

            return View(viewName, _mapper.Map<Department, DepartmentViewModel>(department));
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Edit([FromRoute]int id, DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Take Id from browser only
        public IActionResult Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            try
            {
                _unitOfWork.DepartmentRepository.Delete(_mapper.Map<DepartmentViewModel, Department>(departmentVM));
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(departmentVM);
        }
    }
}
