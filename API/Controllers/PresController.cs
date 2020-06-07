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
        public async Task<ActionResult<PresPoll>> FindPresPoll(int questionId)
        {
            var pres = await _context.PresPolls.FindAsync(questionId);

            if (pres == null)
            {
                return NotFound();
            }

            return pres;
        }

        [HttpGet("party/{party}")]
        public async Task<ActionResult<IEnumerable<PresPoll>>> FindByParty(string party)
        {
           var partyCandidates = await _context.PresPolls.ToListAsync();

            if( partyCandidates == null)
            {
                return NotFound();
            }

            var singleParty = partyCandidates.Where(x => x.CandidateParty == party);

            return singleParty.ToList();



        }

        [HttpGet("party/{party}/candidate/{candidateName}")]
        public async Task<ActionResult<IEnumerable<PresPoll>>> FindByCandidate(string party, string candidateName)
        {
            var partyCandidates = await _context.PresPolls.ToListAsync();

            if (partyCandidates == null)
            {
                return NotFound();
            }

          var singleParty = partyCandidates.Where(x => x.CandidateParty == party && x.CandidateName == candidateName);

            return singleParty.ToList();



        }

        [HttpGet("elections")]
        public async Task<ActionResult<IEnumerable<ElectionDto>>> ElectionPolls()
        {
            var DemCandidate = await _context.PresPolls.ToListAsync();
            var RepCandidate = await _context.PresPolls.ToListAsync();

            if (DemCandidate == null && RepCandidate == null)
            {
                return NotFound();
            }

            var biden = DemCandidate.Where(x => x.CandidateParty == "DEM" && x.CandidateName == "Joseph R. Biden Jr.");
            var trump = RepCandidate.Where(x => x.CandidateParty == "REP" && x.CandidateName == "Donald Trump");
            var election = biden.Join(trump,
                b => b.QuestionId,
                t => t.QuestionId, (b, t) => new ElectionDto
                {
                    QuestionId = b.QuestionId,
                    DemCandidateName = b.CandidateName,
                    RepCandidateName = t.CandidateName,
                    State = b.State,
                    FteGrade = b.FteGrade,
                    SampleSize = b.SampleSize,
                    Methodology = b.Methodology,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Partisan = b.Partisan,
                    RPct = t.Pct,
                    DPct = b.Pct

                });

               

            return election.ToList();



        }

        [HttpGet("elections/{state}")]
        public async Task<ActionResult<IEnumerable<ElectionDto>>> ElectionPollsState(string state)
        {
            var DemCandidate = await _context.PresPolls.ToListAsync();
            var RepCandidate = await _context.PresPolls.ToListAsync();

            if (DemCandidate == null && RepCandidate == null)
            {
                return NotFound();
            }

            var biden = DemCandidate.Where(x => x.CandidateParty == "DEM" && x.CandidateName == "Joseph R. Biden Jr.");
            var trump = RepCandidate.Where(x => x.CandidateParty == "REP" && x.CandidateName == "Donald Trump");
            var election = biden.Join(trump,
                b => b.QuestionId,
                t => t.QuestionId, (b, t) => new ElectionDto
                {
                    QuestionId = b.QuestionId,
                    DemCandidateName = b.CandidateName,
                    RepCandidateName = t.CandidateName,
                    FteGrade = b.FteGrade,
                    State = b.State,
                    SampleSize = b.SampleSize,
                    Methodology = b.Methodology,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Partisan = b.Partisan,
                    RPct = t.Pct,
                    DPct = b.Pct

                });

            var stateElection = election.Where(x => x.State == $"{state}")
                                        .OrderBy(x => x.EndDate).ToList();

            return stateElection;



        }
    }
}
