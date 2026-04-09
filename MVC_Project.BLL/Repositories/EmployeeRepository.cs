using Microsoft.EntityFrameworkCore;
using MVC_Project.BLL.Interfaces;
using MVC_Project.DAL.Contexts;
using MVC_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _dbContext;

        public EmployeeRepository(CompanyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public IQueryable<Employee> GetEmployeesByAdress(string adress)
        //    => _dbContext.Employees.Where(E => E.Address == adress);

        public IQueryable<Employee> GetEmployeesByName(string name)
            => _dbContext.Employees.Include(E => E.Department).Where(E => E.Name!.ToLower().Contains(name.ToLower()));
        //=> _dbContext.Employees.Include(E => E.Department).Where(E => E.Name != null && E.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
    }
}
