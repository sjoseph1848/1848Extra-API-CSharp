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
        public async Task<IActionResult> Import()
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                String.Format("Data/Source/Polls.xlsx"));
                

            using (var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var ep  = new ExcelPackage(stream))
                {
                    // get the first worksheet
                    var ws = ep.Workbook.Worksheets[0];

                    // Initialize the record counters
                    var nPolls = 0;

                    var listPolls = _context.Polls.ToList();

                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {
                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var question = row[nRow, 1].GetValue<int>();

                        // DId we already create a question with this id? 
                        if(listPolls.Where(c => c.QuestionId == question).Count() == 0)
                        {
                            // create a poll entity and fill it with xlsx data
                            var poll = new Poll();
                            poll.QuestionId = question;
                            poll.PollId = row[nRow, 2].GetValue<int>();
                            poll.State = row[nRow, 3].GetValue<string>();
                            poll.PollsterId = row[nRow, 4].GetValue<int>();
                            poll.SponsorIds = row[nRow, 5].GetValue<int?>();
                            poll.DisplayName = row[nRow, 6].GetValue<string>();
                            poll.StartDate = row[nRow, 7].GetValue<DateTime>();
                            poll.EndDate = row[nRow, 8].GetValue<DateTime>();
                            poll.CreatedAt = row[nRow, 9].GetValue<DateTime>();

                            // save it to the database 
                            _context.Polls.Add(poll);
                            await _context.SaveChangesAsync();

                            // store the poll to retrieve 
                            // its Id late on 
                            listPolls.Add(poll);
                            //increment the counter 
                            nPolls++;
                        }
                    }

                    return new JsonResult(new
                    {
                        Poll = nPolls
                    });
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> ImportPres()
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                String.Format("Data/Source/PresPolls.xlsx"));


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

                    // Initialize the record counters
                    var nPres = 0;
                    var lstPolls = _context.Polls.ToList();
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];

                        // create the prespoll entity and fill it with xlsx data 
                        var presPoll = new PresPoll();
                        presPoll.QuestionId = row[nRow, 1].GetValue<int>();
                        presPoll.PollId = row[nRow, 2].GetValue<int>();
                        presPoll.Stage = row[nRow, 3].GetValue<string>();
                        presPoll.RaceId = row[nRow, 4].GetValue<int>();
                        presPoll.State = row[nRow, 5].GetValue<string>();
                        presPoll.CandidateName = row[nRow, 6].GetValue<string>();
                        presPoll.CandidateParty = row[nRow, 7].GetValue<string>();
                        presPoll.pct = row[nRow, 8].GetValue<int>();
                        presPoll.Poll = lstPolls.Where(c => c.QuestionId == presPoll.QuestionId).FirstOrDefault();

                        // save the pres poll to the db 
                        _context.PresPolls.Add(presPoll);
                        await _context.SaveChangesAsync();

                        //increment the counter 
                        nPres++;
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
