using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ContractingPlatform.Models;
using ContractingPlatform.Data;

namespace ContractingPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ContractingPlatformContext _context;

        private List<Entity> EntityList;

        public HomeController(ILogger<HomeController> logger, ContractingPlatformContext context)
        {
            _logger = logger;
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

        public IActionResult Index()
        {
            ViewData["EntityList"] = EntityList;
            ViewData["Contracts"] = _context.Contract.ToList();
            return View();
        }

        [HttpGet]
        public JsonResult Data()
        {
            var result = new Dictionary<string, object>();
            result.Add("entities", EntityList);
            result.Add("contracts", _context.Contract.ToList());
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
