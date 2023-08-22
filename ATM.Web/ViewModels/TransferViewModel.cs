using System.ComponentModel.DataAnnotations;

namespace ATM.Web.ViewModels
{
    public class TransferViewModel
    {
        [Required]
        [Display(Name = "Account No.")]
        public string AccountNo { get; set; }
        [Required]
        [Display(Name = "Re-enter Account No.")]
        [Compare("AccountNo", ErrorMessage = "Account number can't matched")]
        public string ConfirmAccountNo { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [Range(1000, 9999, ErrorMessage = "Enter valid 4 digit PIN")]
        public int PIN { get; set; }
    }
}
