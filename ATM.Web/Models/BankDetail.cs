using System.ComponentModel.DataAnnotations;

namespace ATM.Web.Models
{
    public class BankDetail
    {
        [Key]
        public string AccountNo { get; set; }
        [Required]
        [Range(1000000000000000, 9999999999999999)]
        public long ATMCardNo { get; set; }
        [Required]
        [Range(1000, 9999)]
        public int PIN { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
