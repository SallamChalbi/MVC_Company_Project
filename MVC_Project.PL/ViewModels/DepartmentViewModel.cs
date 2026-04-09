using MVC_Project.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC_Project.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Range(10, 10_000)]
        public int Code { get; set; }
        [Required(ErrorMessage = "Name is Required !!")]
        public string? Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
