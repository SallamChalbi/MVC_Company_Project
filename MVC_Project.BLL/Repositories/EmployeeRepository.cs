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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyDbContext _dbContext;

        public EmployeeRepository(CompanyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
            => _dbContext.Employees.ToList();

        public Employee? GetById(int id)
            => _dbContext.Employees.Find(id);

        public int Update(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            return _dbContext.SaveChanges();
        }
    }
}
