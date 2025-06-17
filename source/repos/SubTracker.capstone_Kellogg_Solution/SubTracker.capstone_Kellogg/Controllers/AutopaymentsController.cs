using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubTracker.capstone_Kellogg.Data;
using SubTracker.capstone_Kellogg.Models;
using SubTracker.capstone_Kellogg.Services;


namespace SubTracker.capstone_Kellogg.Controllers
{
    public class AutopaymentsController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly AutopaymentProcessor _processor;

        public AutopaymentsController(ProjectDbContext context, AutopaymentProcessor processor)
        {
            _context = context;
            _processor = processor;
        }

        // GET: Autopayments
        public async Task<IActionResult> Index()
        {
            var autopayments = _context.Autopayments.Include(a => a.Account);
            return View(await autopayments.ToListAsync());
        }

        // GET: Autopayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autopayment = await _context.Autopayments
                .FirstOrDefaultAsync(m => m.AutopaymentId == id);
            if (autopayment == null)
            {
                return NotFound();
            }

            return View(autopayment);
        }

        // GET: Autopayments/Create
        public IActionResult Create(int? accountId)
        {
            if (accountId == null || !_context.Accounts.Any(a => a.AccountId == accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            var autopayment = new Autopayment
            {
                AccountId = accountId.Value,
                StartDate = DateTime.Today
            };

            return View(autopayment);
        }



        // POST: Autopayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutopaymentId,AccountId,Name,Amount,StartDate,FrequencyInterval,FrequencyUnit")] Autopayment autopayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autopayment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home", new { accountId = autopayment.AccountId });
            }
            return View(autopayment);
        }


        // GET: Autopayments/Edit/5
        // GET: Autopayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autopayment = await _context.Autopayments.FindAsync(id);
            if (autopayment == null)
            {
                return NotFound();
            }

            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountName", autopayment.AccountId);
            return View(autopayment);
        }


        // POST: Autopayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutopaymentId,AccountId,Name,Amount,StartDate,FrequencyInterval, FrequencyUnit")] Autopayment autopayment)
        {
            if (id != autopayment.AutopaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autopayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutopaymentExists(autopayment.AutopaymentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autopayment);
        }

        // GET: Autopayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autopayment = await _context.Autopayments
                .FirstOrDefaultAsync(m => m.AutopaymentId == id);
            if (autopayment == null)
            {
                return NotFound();
            }

            return View(autopayment);
        }

        // POST: Autopayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autopayment = await _context.Autopayments.FindAsync(id);
            if (autopayment != null)
            {
                _context.Autopayments.Remove(autopayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutopaymentExists(int id)
        {
            return _context.Autopayments.Any(e => e.AutopaymentId == id);
        }

        [HttpPost]
        public async Task<IActionResult> RunAutopaymentProcessor()
        {
            var count = await _processor.RunAsync();
            TempData["Message"] = $"{count} autopayments processed.";
            return RedirectToAction("Index", "Accounts");
        }

    }
}
