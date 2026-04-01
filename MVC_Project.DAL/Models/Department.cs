using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Range(10,10_000)]
        public int Code { get; set; }
        [Required(ErrorMessage = "Name is Required !!")]
        public string? Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
