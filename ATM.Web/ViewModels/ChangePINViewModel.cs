using System.ComponentModel.DataAnnotations;

namespace ATM.Web.ViewModels
{
    public class ChangePINViewModel
    {
        [Required]
        [Display(Name = "Current PIN")]
        [Range(1000, 9999, ErrorMessage = "Enter valid 4 digit PIN")]
        public int OldPIN { get; set; }
        [Required]
        [Display(Name = "New PIN")]
        [Range(1000, 9999, ErrorMessage = "Enter valid 4 digit PIN")]
        public int NewPIN { get; set; }
        [Required]
        [Display(Name = "Confirm new PIN")]
        [Range(1000, 9999, ErrorMessage = "Enter valid 4 digit PIN")]
        [Compare("NewPIN", ErrorMessage = "Confirm PIN doesn't matched with new PIN")]
        public int ConfirmNewPIN { get; set; }
    }
}
