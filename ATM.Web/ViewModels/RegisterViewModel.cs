using ATM.Web.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ATM.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Enter 10 digit contact number only")]
        [MaxLength(10, ErrorMessage = "Enter 10 digit contact number only")]
        public string ContactNo { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Enter length is too long")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password length must be greater than 6")]
        [MaxLength(13, ErrorMessage = "Password length must be less than 13")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't matched")]
        public string ConfirmPassword { get; set; }
    }
}
