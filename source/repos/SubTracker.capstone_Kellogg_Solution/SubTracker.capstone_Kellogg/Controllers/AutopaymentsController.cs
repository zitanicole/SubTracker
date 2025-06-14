using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubTracker.capstone_Kellogg.Data;
using SubTracker.capstone_Kellogg.Models;

namespace SubTracker.capstone_Kellogg.Controllers
{
    public class AutopaymentsController : Controller
    {
        private readonly ProjectDbContext _context;

        public AutopaymentsController(ProjectDbContext context)
        {
            _context = context;
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
        // GET: Autopayments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountName");
            return View();
        }


        // POST: Autopayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutopaymentId,AccountId,Name,Amount,StartDate,Frequency")] Autopayment autopayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autopayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountName", autopayment.AccountId);
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
        public async Task<IActionResult> Edit(int id, [Bind("AutopaymentId,AccountId,Name,Amount,StartDate,Frequency")] Autopayment autopayment)
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
    }
}
