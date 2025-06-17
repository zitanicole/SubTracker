using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;



namespace SubTracker.capstone_Kellogg.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public int AccountId { get; set; } // links to Account
      
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } // "Deposit" or "Withdrawal"

        [Required]
        public string Description { get; set; }

        [BindNever]
        public virtual Account? Account { get; set; }
    }
}
