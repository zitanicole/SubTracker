using System.Collections.Generic;

namespace SubTracker.capstone_Kellogg.Models
{
    public class ViewBalanceViewModel
    {
        public decimal CurrentBalance { get; set; }
        public List<Transaction> RecentTransactions { get; set; }
    }
}
