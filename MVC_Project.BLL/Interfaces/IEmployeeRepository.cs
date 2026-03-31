using MVC_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.BLL.Interfaces
{
    public interface IEmployeeRepository: IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAdress(string adress);
    }
}
