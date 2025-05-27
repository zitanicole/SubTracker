using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubTracker.capstone_Kellogg.Models;
using SubTracker.capstone_Kellogg.Data;

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

    public IActionResult ViewBalance()
    {
        var account = _context.Accounts.FirstOrDefault();

        var transactions = _context.Transactions
            .OrderByDescending(t => t.Date)
            .Take(5)
            .ToList();

        var model = new ViewBalanceViewModel
        {
            CurrentBalance = account?.Balance ?? 0,
            RecentTransactions = transactions
        };

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
