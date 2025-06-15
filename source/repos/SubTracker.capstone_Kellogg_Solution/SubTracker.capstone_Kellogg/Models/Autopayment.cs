using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubTracker.capstone_Kellogg.Models
{
    public class Autopayment
    {
        public int AutopaymentId { get; set; }
        [Required]
        public int AccountId { get; set; } // link to Account
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        public string Frequency { get; set; } // Daily, Weekly, Monthly, Yearly
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

    }
}
