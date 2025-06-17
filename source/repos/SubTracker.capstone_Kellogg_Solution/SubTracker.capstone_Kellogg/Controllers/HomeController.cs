using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubTracker.capstone_Kellogg.Models;
using SubTracker.capstone_Kellogg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubTracker.capstone_Kellogg.ViewModels;
using SubTracker.capstone_Kellogg.Services;


namespace SubTracker.capstone_Kellogg.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectDbContext _context;
        private readonly AutopaymentProcessor _autopaymentProcessor;

        public HomeController(ILogger<HomeController> logger, ProjectDbContext context, AutopaymentProcessor autopaymentProcessor)
        {
            _logger = logger;
            _context = context;
            _autopaymentProcessor = autopaymentProcessor;
        }

        public async Task<IActionResult> Index(int? accountId)
        {
            var model = new HomeViewModel
            {
                Accounts = await _context.Accounts.ToListAsync(),
                Transactions = new List<Transaction>()
            };

            if (accountId != null)
            {
                await _autopaymentProcessor.RunAsync();
                model.SelectedAccount = await _context.Accounts.FindAsync(accountId);
                model.Transactions = await _context.Transactions
                    .Where(t => t.AccountId == accountId)
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ViewBalance(int? accountId)
        {
            var accounts = await _context.Accounts
                .Select(a => new { a.AccountId, a.AccountName })
                .ToListAsync();

            var selectList = new SelectList(accounts, "AccountId", "AccountName", accountId);

            if (accountId == null)
            {
                if (accounts.Any())
                {
                    accountId = accounts.First().AccountId;
                }
                else
                {
                    return View(new ViewBalanceViewModel
                    {
                        AccountId = 0,
                        CurrentBalance = 0,
                        RecentTransactions = new List<Transaction>(),
                        Accounts = selectList
                    });
                }
            }

            decimal balance = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .SumAsync(t => t.Type == "Deposit" ? t.Amount : t.Amount * -1);

            var recentTransactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToListAsync();

            var model = new ViewBalanceViewModel
            {
                AccountId = accountId.Value,
                CurrentBalance = balance,
                RecentTransactions = recentTransactions,
                Accounts = selectList
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
