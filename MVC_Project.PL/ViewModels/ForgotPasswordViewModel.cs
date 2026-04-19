using System.ComponentModel.DataAnnotations;

namespace MVC_Project.PL.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }
    }
}
