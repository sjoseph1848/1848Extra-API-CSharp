using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidHospitalizationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CovidHospitalizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospByCounty>>> GetHospitalizationsByCounty()
        {
            return await _context.CountyHospitalizations.ToListAsync();
        }


        [HttpGet("hospitalizations/{county}")]
        public async Task<ActionResult<IEnumerable<HospByCounty>>> FindByCounty(string county)
        {
            var counties = await _context.CountyHospitalizations.ToListAsync();

            if (counties == null)
            {
                return NotFound();
            }

            var covidCounty = counties.Where(x => x.County == county);
            if (covidCounty.Count() == 0)
            {
                covidCounty = from s in counties
                              where EF.Functions.Like(s.County, $"%{county}%")
                              select s;
            }

            return covidCounty.ToList();

        }


    }
}
