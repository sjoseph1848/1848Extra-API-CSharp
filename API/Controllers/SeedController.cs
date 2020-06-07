using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SeedController(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ImportPres()
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                String.Format("Data/Source/PresPolls672020.xlsx"));


            using (var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var ep = new ExcelPackage(stream))
                {
                    // get the first worksheet
                    var ws = ep.Workbook.Worksheets[0];
                    var lstPresPolls = _context.PresPolls.ToList();

                 
                    // Initialize the record counters
                    var nPres = 0;
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var questionId = row[nRow, 1].GetValue<int>();

                        if(lstPresPolls.Where(q => q.QuestionId == questionId).Count() < 2)
                        {
                            var presPoll = new PresPoll();
                            presPoll.QuestionId = row[nRow, 1].GetValue<int>();
                            presPoll.PollId = row[nRow, 2].GetValue<int>();
                            presPoll.State = row[nRow, 3].GetValue<string>();
                            presPoll.PollsterId = row[nRow, 4].GetValue<long>();
                            presPoll.SponsorId = row[nRow, 5].GetValue<long>();
                            presPoll.PollsterRatingId = row[nRow, 6].GetValue<long>();
                            presPoll.FteGrade = row[nRow, 7].GetValue<string>();
                            presPoll.SampleSize = row[nRow, 8].GetValue<int>();
                            presPoll.Methodology = row[nRow, 9].GetValue<string>();
                            presPoll.StartDate = row[nRow, 10].GetValue<DateTime>();
                            presPoll.EndDate = row[nRow, 11].GetValue<DateTime>();
                            presPoll.Partisan = row[nRow, 12].GetValue<string>();
                            presPoll.RaceId = row[nRow, 13].GetValue<int>();
                            presPoll.CandidateName = row[nRow, 14].GetValue<string>();
                            presPoll.CandidateParty = row[nRow, 15].GetValue<string>();
                            presPoll.Pct = row[nRow, 16].GetValue<int>();
                            // create the prespoll entity and fill it with xlsx data 
                            // save the pres poll to the db 
                            _context.PresPolls.Add(presPoll);
                            await _context.SaveChangesAsync();

                            //increment the counter 
                            nPres++;
                        }
                        
                    }

                    return new JsonResult(new
                    {
                       PresPoll = nPres
                    });
                }

            }
        }
    }
}
