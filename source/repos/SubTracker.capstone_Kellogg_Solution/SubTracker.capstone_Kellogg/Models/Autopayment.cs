using System.ComponentModel.DataAnnotations.Schema;

namespace SubTracker.capstone_Kellogg.Models
{
    public class Autopayment
    {
        public int AutopaymentId { get; set; }
        public int AccountId { get; set; } // link to Account
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public string Frequency { get; set; } // Daily, Weekly, Monthly, Yearly
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

    }
}
