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
    public class CarrierController : Controller
    {
        private readonly ContractingPlatformContext _context;

        public CarrierController(ContractingPlatformContext context)
        {
            _context = context;
        }

        // GET: Carrier
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carrier.ToListAsync());
        }

        // GET: Carrier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = await _context.Carrier
                .FirstOrDefaultAsync(m => m.CarrierId == id);
            if (carrier == null)
            {
                return NotFound();
            }

            return View(carrier);
        }

        // GET: Carrier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carrier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarrierId,Name,Address,Phone")] Carrier carrier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carrier);
        }

        // GET: Carrier/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = await _context.Carrier.FindAsync(id);
            if (carrier == null)
            {
                return NotFound();
            }
            return View(carrier);
        }

        // POST: Carrier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarrierId,Name,Address,Phone")] Carrier carrier)
        {
            if (id != carrier.CarrierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarrierExists(carrier.CarrierId))
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
            return View(carrier);
        }

        // GET: Carrier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = await _context.Carrier
                .FirstOrDefaultAsync(m => m.CarrierId == id);
            if (carrier == null)
            {
                return NotFound();
            }

            return View(carrier);
        }

        // POST: Carrier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrier = await _context.Carrier.FindAsync(id);
         
            var contracts =  _context.Contract.Where(c=> (c.EntityA == "C" + carrier.CarrierId) ||  (c.EntityB == "C" + carrier.CarrierId));
           
            _context.Carrier.Remove(carrier);

            if(contracts.Count() != 0) {
                _context.Contract.RemoveRange(contracts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarrierExists(int id)
        {
            return _context.Carrier.Any(e => e.CarrierId == id);
        }
    }
}
