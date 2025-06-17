using SubTracker.capstone_Kellogg.Data;
using SubTracker.capstone_Kellogg.Models;
using Microsoft.EntityFrameworkCore;

namespace SubTracker.capstone_Kellogg.Services
{
    public class AutopaymentProcessor
    {
        private readonly ProjectDbContext _context;

        public AutopaymentProcessor(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> RunAsync()
        {
            var today = DateTime.Today;
            var autopayments = await _context.Autopayments
                .Include(ap => ap.Account)
                .ToListAsync();

            int paymentsMade = 0;

            foreach (var ap in autopayments)
            {
                var lastPayment = await _context.Transactions
                    .Where(t => t.AccountId == ap.AccountId && t.Description == ap.Name)
                    .OrderByDescending(t => t.Date)
                    .FirstOrDefaultAsync();

                var lastPaidDate = lastPayment?.Date ?? ap.StartDate;

                DateTime nextDue = ap.FrequencyUnit switch
                {
                    "Week" => lastPaidDate.AddDays(7 * ap.FrequencyInterval),
                    "Month" => lastPaidDate.AddMonths(ap.FrequencyInterval),
                    "Year" => lastPaidDate.AddYears(ap.FrequencyInterval),
                    _ => DateTime.MaxValue
                };

                while (nextDue <= today)
                {
                    _context.Transactions.Add(new Transaction
                    {
                        AccountId = ap.AccountId,
                        Amount = ap.Amount,
                        Type = "Withdrawal",
                        Date = nextDue,
                        Description = ap.Name
                    });

                    ap.Account.Balance -= ap.Amount;
                    paymentsMade++;

                    // advance to next due date
                    nextDue = ap.FrequencyUnit switch
                    {
                        "Week" => nextDue.AddDays(7 * ap.FrequencyInterval),
                        "Month" => nextDue.AddMonths(ap.FrequencyInterval),
                        "Year" => nextDue.AddYears(ap.FrequencyInterval),
                        _ => DateTime.MaxValue
                    };
                }

            }

            await _context.SaveChangesAsync();
            return paymentsMade;
        }
    }
}
