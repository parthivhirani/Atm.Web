using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Web.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

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

        [ForeignKey("AccountNo")]
        public BankDetail BankDetails { get; set; }

        public string PasswordHash { get; set; }
    }
}
