using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidCasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CovidCasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CasesByCounty>>> GetCasesByCounty()
        {
            return await _context.CasesByCounty.ToListAsync();
        }

        [HttpGet("deaths/{county}")]
        public async Task<ActionResult<IEnumerable<CasesByCounty>>> FindByCounty(string county)
        {
            var counties = await _context.CasesByCounty.ToListAsync();

            if (counties == null)
            {
                return NotFound();
            }

            var caseCounty = counties.Where(x => x.County == county);
            if (caseCounty.Count() == 0)
            {
                caseCounty = from s in counties
                              where EF.Functions.Like(s.County, $"%{county}%")
                              select s;
            }

            return caseCounty.ToList();
        }
    }
}
