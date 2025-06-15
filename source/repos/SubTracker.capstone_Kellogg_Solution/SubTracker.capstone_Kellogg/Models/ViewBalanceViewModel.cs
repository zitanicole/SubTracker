using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SubTracker.capstone_Kellogg.Models
{
    public class ViewBalanceViewModel
    {
        public int AccountId { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<Transaction> RecentTransactions { get; set; }
        public SelectList Accounts { get; set; }
    }
}
