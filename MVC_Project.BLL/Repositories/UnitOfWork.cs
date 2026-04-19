using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _dbContext;
        private readonly Lazy<IDepartmentRepository> departmentRepository; // Lazy<T> will initialize T object only when used it (For Department or Employee )
        private readonly Lazy<IEmployeeRepository> employeeRepository; // will not initialize the two object (Employee/Department Repository) every time when creating object from UnitOfWork

        public IDepartmentRepository DepartmentRepository => departmentRepository.Value;
        public IEmployeeRepository EmployeeRepository => employeeRepository.Value; 
        public UnitOfWork(CompanyDbContext dbContext)
        {
            departmentRepository = new Lazy<IDepartmentRepository>(new DepartmentRepository(dbContext));
            employeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(dbContext));
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
