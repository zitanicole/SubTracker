using SubTracker.capstone_Kellogg.Models;
using System.Collections.Generic;

namespace SubTracker.capstone_Kellogg.ViewModels
{
    public class HomeViewModel
    {
        public List<Account> Accounts { get; set; }
        public Account SelectedAccount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}

