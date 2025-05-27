using System.ComponentModel.DataAnnotations.Schema;

namespace SubTracker.capstone_Kellogg.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }   // stores current balance
    }
}
