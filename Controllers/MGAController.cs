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
    public class MGAController : Controller
    {
        private readonly ContractingPlatformContext _context;

        public MGAController(ContractingPlatformContext context)
        {
            _context = context;
        }

        // GET: MGA
        public async Task<IActionResult> Index()
        {
            return View(await _context.MGA.ToListAsync());
        }

        // GET: MGA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mGA = await _context.MGA
                .FirstOrDefaultAsync(m => m.MgaId == id);
            if (mGA == null)
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
                    .Union((from m in _context.MGA
                    select new {
                        Id = "M" + m.MgaId,
                        m.Name
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

            var contracts = adjacencyList.ContainsKey("M"+id) ? adjacencyList["M"+id] : new List<string>();

            if(contracts.Count() == 0) {
                ViewData["Message"] = "No contracts exist";
            }
            ViewData["Contracts"] = contracts;
            ViewData["EntityList"] = entityList;

            return View(mGA);
        }

        // GET: MGA/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MGA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MgaId,Name,Address,Phone")] MGA mGA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mGA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mGA);
        }

        // GET: MGA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mGA = await _context.MGA.FindAsync(id);
            if (mGA == null)
            {
                return NotFound();
            }
            return View(mGA);
        }

        // POST: MGA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MgaId,Name,Address,Phone")] MGA mGA)
        {
            if (id != mGA.MgaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mGA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MGAExists(mGA.MgaId))
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
            return View(mGA);
        }

        // GET: MGA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mGA = await _context.MGA
                .FirstOrDefaultAsync(m => m.MgaId == id);
            if (mGA == null)
            {
                return NotFound();
            }

            return View(mGA);
        }

        // POST: MGA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mGA = await _context.MGA.FindAsync(id);         

            var contracts =  _context.Contract.Where(c=> (c.EntityA == "M" + mGA.MgaId) ||  (c.EntityB == "M" + mGA.MgaId));
           
            _context.MGA.Remove(mGA);

            if(contracts.Count() != 0) {
                _context.Contract.RemoveRange(contracts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MGAExists(int id)
        {
            return _context.MGA.Any(e => e.MgaId == id);
        }
    }
}
