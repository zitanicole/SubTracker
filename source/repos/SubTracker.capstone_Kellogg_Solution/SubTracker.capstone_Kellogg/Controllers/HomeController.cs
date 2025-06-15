using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubTracker.capstone_Kellogg.Models;
using SubTracker.capstone_Kellogg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SubTracker.capstone_Kellogg.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProjectDbContext _context;

    public HomeController(ILogger<HomeController> logger, ProjectDbContext context)
    {
        _logger = logger;
        _context = context; 
    }

    public IActionResult Index()
    {
        return View();
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
                // Automatically use the first account
                accountId = accounts.First().AccountId;
            }
            else
            {
                // No accounts exist — return empty balance model
                return View(new ViewBalanceViewModel
                {
                    AccountId = 0,
                    CurrentBalance = 0,
                    RecentTransactions = new List<Transaction>(),
                    Accounts = selectList
                });
            }
        }


        var latestTransaction = await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .FirstOrDefaultAsync();

        if (latestTransaction == null)
        {
            return View(new ViewBalanceViewModel
            {
                AccountId = accountId.Value,
                CurrentBalance = 0,
                RecentTransactions = new List<Transaction>(),
                Accounts = selectList
            });
        }

        decimal balance = latestTransaction.Amount;
        DateTime balanceStartDate = latestTransaction.Date;

        var autopayments = await _context.Autopayments
            .Where(a => a.AccountId == accountId &&
                        a.StartDate > balanceStartDate &&
                        a.StartDate <= DateTime.Today)
            .ToListAsync();

        foreach (var auto in autopayments)
        {
            balance -= auto.Amount;
        }

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
