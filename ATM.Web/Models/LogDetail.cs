using System.ComponentModel.DataAnnotations;

namespace ATM.Web.Models
{
    public class LogDetail
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public string TransactionType { get; set; }
        [Required]
        [MaxLength(20)]
        public string FromAccount { get; set; }
        [MaxLength(20)]
        public string? ToAccount { get; set; }
        [Required]
        public int AmountTransferred { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfTransaction { get; set; }

    }
}
