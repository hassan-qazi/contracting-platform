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
    public class ContractController : Controller
    {
        private readonly ContractingPlatformContext _context;

        private List<Entity> EntityList;

        public ContractController(ContractingPlatformContext context)
        {
            _context = context;

            var query =                     
                    ((from carrier in _context.Carrier
                    select new {
                        Id = "C" + carrier.CarrierId,
                        carrier.Name
                    }).OrderBy(e=>e.Name))
                    .Union((from mga in _context.MGA
                    select new {
                        Id = "M" + mga.MgaId,
                        mga.Name
                    }).OrderBy(e=>e.Name))
                    .Union((from advisor in _context.Advisor
                    select new {
                        Id = "A" + advisor.AdvisorId,
                        Name = advisor.FirstName + " " + advisor.LastName
                    }).OrderBy(e=>e.Name))
                    .Select(entity => new  Entity() {
                        Id = entity.Id,
                        Name = entity.Name
                    })
                    ;

            EntityList = query.ToList();
            //ViewData["EntityList"] = EntityList;

        }

        // GET: Contract
        public async Task<IActionResult> Index()
        {
            ViewData["EntityList"] = EntityList;
            
            return View(await _context.Contract.ToListAsync());
        }

        // GET: Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
            {
                return NotFound();
            }

            ViewData["EntityList"] = EntityList;           

            return View(contract);
        }

        // GET: Contract/Create
        public IActionResult Create()
        {    
            ViewData["EntityList"] = new SelectList(EntityList,"Id","Name");
            return View();
        }

        // POST: Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,EntityA,EntityB")] Contract contract)
        {
            var contractWithSelf = (contract.EntityA == contract.EntityB);

            var duplicateContract = _context.Contract.ToList().Exists(c => 
                                                                        (c.EntityA == contract.EntityA && 
                                                                         c.EntityB == contract.EntityB) ||
                                                                        (c.EntityA == contract.EntityB && 
                                                                         c.EntityB == contract.EntityA) 
                                                                     );

            if (ModelState.IsValid && !contractWithSelf && !duplicateContract)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            if(contractWithSelf) {
                ViewData["Error"] = "Can not create a contract with self";
            }
            if(duplicateContract) {
                ViewData["Error"] = "Duplicate contract";
            }
            
            ViewData["EntityList"] = new SelectList(EntityList,"Id","Name");

            return View(contract);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            ViewData["EntityList"] = EntityList;
            return View(contract);
        }

        // POST: Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractId,EntityA,EntityB")] Contract contract)
        {
            if (id != contract.ContractId)
            {
                return NotFound();
            }

            var contractWithSelf = (contract.EntityA == contract.EntityB);

            var duplicateContract =  _context.Contract.AsNoTracking().ToList().Exists(c => 
                                                                        (c.EntityA == contract.EntityA && 
                                                                         c.EntityB == contract.EntityB) ||
                                                                        (c.EntityA == contract.EntityB && 
                                                                         c.EntityB == contract.EntityA) 
                                                                    );

            if (ModelState.IsValid && !contractWithSelf && !duplicateContract)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractId))
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

            if(contractWithSelf) {
                ViewData["Error"] = "Can not create a contract with self";
            }
            if(duplicateContract) {
                ViewData["Error"] = "Duplicate contract";
            }
            
            ViewData["EntityList"] = EntityList;

            return View(contract);
        }

        // GET: Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["EntityList"] = EntityList;    

            return View(contract);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Contract/Chain
        public IActionResult Chain()
        {   
            ViewData["EntityList"] = EntityList;
            return View();
        }

        // POST: Contract/Chain
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Chain([Bind("EntityA,EntityB")] Contract contract)
        {  
            var contractWithSelf = (contract.EntityA == contract.EntityB);

            if(contractWithSelf) {
                ViewData["Error"] = "No contract chains with self";
            }
            else {
               var contractsAB = _context.Contract.Where(c=> (c.EntityA == contract.EntityA &&
                                                            c.EntityB == contract.EntityB) ||
                                                            (c.EntityB == contract.EntityA && 
                                                             c.EntityA == contract.EntityB)
                                                      );
               
               if(contractsAB.Count() != 0) {
                   ViewData["Message"] = EntityList.Find(e=>e.Id == contract.EntityA).Name + " -- " + 
                                         EntityList.Find(e=>e.Id == contract.EntityB).Name;
               }
               else {
                   var contractsA = _context.Contract.Where(c=> (c.EntityA == contract.EntityA) || 
                                                                (c.EntityB == contract.EntityA)).ToList();
                   if(contractsA.Count() != 0) {
                       var contractsB = _context.Contract.Where(c=> (c.EntityA == contract.EntityB) || 
                                                                (c.EntityB == contract.EntityB)).ToList();
                        if(contractsB.Count() != 0) {

                            //contracts exist for entityA and entityB, find the smallest chain if it exists
                                                        
                            var graph = new Graph();

                            foreach(var c in _context.Contract) {
                                graph.AddEdge(c.EntityA,c.EntityB);
                            }

                            var shortestPath = graph.ShortestPath(contract.EntityA, contract.EntityB);

                            if(shortestPath.Count() == 0) {
                                ViewData["Message"] = "No contract chains found";
                            }
                            else {

                                var shortestPathList = shortestPath.ToList();

                                var message = EntityList.Find(e=>e.Id == shortestPathList[0]).Name;

                                for(int i=1; i < shortestPathList.Count(); i++) {
                                    message += " -- " +  EntityList.Find(e=>e.Id == shortestPathList[i]).Name;
                                }

                                ViewData["Message"] = message;
                            }

                            

                        }
                        else {
                            ViewData["Error"] = "No contracts exist for " + EntityList.Find(e=>e.Id == contract.EntityB).Name;
                        }

                   }
                   else {
                        ViewData["Error"] = "No contracts exist for " + EntityList.Find(e=>e.Id == contract.EntityA).Name;
                   }


                  
               }
            }

            ViewData["EntityList"] = EntityList;    

            return View(contract);
        }

        private bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.ContractId == id);
        }
    }
}
