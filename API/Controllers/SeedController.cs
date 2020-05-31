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
    [Route("api/[controller]")]
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
    }
}
