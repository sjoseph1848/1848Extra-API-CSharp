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
    public class CovidDeathsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CovidDeathsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeathByCounty>>> GetDeathsByCounty()
        {
            return await _context.DeathsByCounty.ToListAsync();
        }

        [HttpGet("deaths/{county}")]
        public async Task<ActionResult<IEnumerable<DeathByCounty>>> FindByCounty(string county)
        {
            var counties = await _context.DeathsByCounty.ToListAsync();

            if (counties == null)
            {
                return NotFound();
            }

            var deathCounty = counties.Where(x => x.County == county);
            if (deathCounty.Count() == 0)
            {
                deathCounty = from s in counties
                              where EF.Functions.Like(s.County, $"%{county}%")
                              select s;
            }

            return deathCounty.ToList();
        }
    }
}
