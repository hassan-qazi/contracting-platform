using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractingPlatform.Data;
using ContractingPlatform.Models;

namespace ContractingPlatform.Controllers
{
    public class AdvisorController : Controller
    {
        private readonly ContractingPlatformContext _context;

        public AdvisorController(ContractingPlatformContext context)
        {
            _context = context;
        }

        // GET: Advisor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Advisor.ToListAsync());
        }

        // GET: Advisor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisor
                .FirstOrDefaultAsync(m => m.AdvisorId == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // GET: Advisor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Advisor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdvisorId,FirstName,LastName,Address,Phone,Health")] Advisor advisor)
        {
            if (ModelState.IsValid)
            {               
                _context.Add(advisor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advisor);
        }

        // GET: Advisor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisor.FindAsync(id);
            if (advisor == null)
            {
                return NotFound();
            }
            return View(advisor);
        }

        // POST: Advisor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdvisorId,FirstName,LastName,Address,Phone,Health")] Advisor advisor)
        {
            if (id != advisor.AdvisorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorExists(advisor.AdvisorId))
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
            return View(advisor);
        }

        // GET: Advisor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisor
                .FirstOrDefaultAsync(m => m.AdvisorId == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // POST: Advisor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advisor = await _context.Advisor.FindAsync(id);
            var contracts =  _context.Contract.Where(c=> (c.EntityA == "A" + advisor.AdvisorId) ||  (c.EntityB == "A" + advisor.AdvisorId));
           
            _context.Advisor.Remove(advisor);

            if(contracts.Count() != 0) {
                _context.Contract.RemoveRange(contracts);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AdvisorExists(int id)
        {
            return _context.Advisor.Any(e => e.AdvisorId == id);
        }
    }
}
