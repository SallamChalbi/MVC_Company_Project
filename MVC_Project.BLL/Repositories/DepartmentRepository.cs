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
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _dbContext;

        public DepartmentRepository(CompanyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Department department)
        {
            _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();
        }

        public int Delete(Department department)
        {
            _dbContext.Departments.Remove(department);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
            => _dbContext.Departments.ToList();

        public Department? GetById(int id)
            => _dbContext.Find<Department>(id);

        public int Update(Department department)
        {
            _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }
    }
}
