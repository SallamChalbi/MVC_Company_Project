using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Characters")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 Characters")]
        public string? Name { get; set; }
        [Range(22, 40, ErrorMessage = "Age must be in Range from 22 to 40")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Adress is Required!")]
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Adress must be Like \'123-Street-City-Country\'")]
        public string? Address { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
