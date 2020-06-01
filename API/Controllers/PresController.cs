using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class PresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresPoll>>> GetPresPolls()
        {
            return await _context.PresPolls.ToListAsync();
        }

        [HttpGet("{questionId}")]
        public async Task<ActionResult<PresPoll>> GetPresPoll(int questionId)
        {
            var pres = await _context.PresPolls.FindAsync(questionId);

            if (pres == null)
            {
                return NotFound();
            }

            return pres;
        }
    }
}
