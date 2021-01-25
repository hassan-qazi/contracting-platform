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

            var graph = new Graph();
            foreach(var c in _context.Contract) {
                graph.AddEdge(c.EntityA,c.EntityB);
            }
            var adjacencyList = graph.AdjacencyList;

            var query =                     
                    ((from c in _context.Carrier
                    select new {
                        Id = "C" + c.CarrierId,
                        c.Name
                    }).OrderBy(e=>e.Name))
                    .Union((from mga in _context.MGA
                    select new {
                        Id = "M" + mga.MgaId,
                        mga.Name
                    }).OrderBy(e=>e.Name))
                    .Union((from adv in _context.Advisor
                    select new {
                        Id = "A" + adv.AdvisorId,
                        Name = adv.FirstName + " " + adv.LastName
                    }).OrderBy(e=>e.Name))
                    .Select(entity => new  Entity() {
                        Id = entity.Id,
                        Name = entity.Name
                    })
                    ;

            var entityList = query.ToList();

            var contracts = adjacencyList.ContainsKey("C"+id) ? adjacencyList["C"+id] : new List<string>();

            if(contracts.Count() == 0) {
                ViewData["Message"] = "No contracts exist";
            }
            ViewData["Contracts"] = contracts;
            ViewData["EntityList"] = entityList;

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
