using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            IWebHostEnvironment env
            )
        {
            _context = context;
            _env = env;
            
        }
        [HttpGet]
        public async Task<IActionResult> ImportDeathByCounty()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Covid/Death/TexasCOVID19Deaths624.xlsx"));

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
                    var lstDeathDates = _context.DeathsByCounty.ToList();

                    // Initialize the record counters
                    var nDeaths = 0;
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var dateId = row[nRow, 4].GetValue<DateTime>();

                        if (lstDeathDates.Where(d => d.Date == dateId).Count() == 0)
                        {
                            var deathByCounty = new DeathByCounty();
                            deathByCounty.Date = row[nRow, 4].GetValue<DateTime>();
                            deathByCounty.County = row[nRow, 1].GetValue<string>();
                            deathByCounty.Deaths = row[nRow, 3].GetValue<int>();

                            // create the hospbycounty entity and fill it with xlsx data 
                            // save the hospbycounty to the db 
                            _context.DeathsByCounty.Add(deathByCounty);
                            await _context.SaveChangesAsync();

                            //increment the counter 
                            nDeaths++;
                        }

                    }

                    return new JsonResult(new
                    {
                        DeathByCounty = nDeaths
                    }); 
                }

            }
        }


        [HttpGet]
        public async Task<IActionResult> ImportCasesByCounty()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Covid/Cases/TexasCOVID19CaseCountDatabyCounty624.xlsx"));

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
                    var lstCountyDates = _context.CasesByCounty.ToList();

                    // Initialize the record counters
                    var nCountyCases = 0;
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var dateId = row[nRow, 4].GetValue<DateTime>();

                        if (lstCountyDates.Where(d => d.Date == dateId).Count() == 0)
                        {
                            var casesByCounty = new CasesByCounty();
                            casesByCounty.Date = row[nRow, 4].GetValue<DateTime>();
                            casesByCounty.County = row[nRow, 1].GetValue<string>();
                            casesByCounty.Cases = row[nRow, 3].GetValue<int>();

                            // create the hospbycounty entity and fill it with xlsx data 
                            // save the hospbycounty to the db 
                            _context.CasesByCounty.Add(casesByCounty);
                            await _context.SaveChangesAsync();

                            //increment the counter 
                            nCountyCases++;
                        }

                    }

                    return new JsonResult(new
                    {
                        CasesByCounty = nCountyCases
                    });
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> FormatCasesByCron()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Trials/TexasCases.xlsx"));

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
                    var lstCounties = _context.CasesByCounty.Select(s => s.County).Distinct().ToList();
                    int nums = lstCounties.Count;
                    int rowCnt = ws.Dimension.End.Row;
                    int colCnt = ws.Dimension.End.Column + 1;
                    DateTime startingDate = new DateTime(2020, 03, 04);
                    List<CasesByCounty> CasesYall = new List<CasesByCounty>();
                    var nCountyCases = 0;
                    for ( var i = 0; i < lstCounties.Count; i++)
                    {
                        for(var j = 4; j < 258; j++)
                        {
                            if(lstCounties[i] == ws.Cells[j, 1].Value.ToString())
                            {
                                for(var k = 35; k < colCnt; k++)
                                {
                                    var casesByCounty = new CasesByCounty();
                                    casesByCounty.Date = startingDate.AddDays(k).Date;
                                    casesByCounty.County = lstCounties[i];
                                    casesByCounty.Cases = ws.Cells[j,k].GetValue<int>();
                                    CasesYall.Add(casesByCounty);
                                    
                                }
                            }
                        }
                    }

                    var dallas = CasesYall.Where(x => x.County == "Dallas");
                    // Initialize the record counters

                    // iterate through all rows, skipping the first one


                    return new JsonResult(new
                    {
                        dallas
                    });
                }

            }
        }


        [HttpGet]
        public async Task<IActionResult> ImportCasesByCron()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/Trials/TexasCases.xlsx"));

            //var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var client = new HttpClient();
            var response = await client.GetAsync(@"https://dshs.texas.gov/coronavirus/TexasCOVID19DailyCountyCaseCountData.xlsx");

                using (var streamer = await response.Content.ReadAsStreamAsync())
                {
                    var fileInfo = new FileInfo(path);
                    using (var fileStream = fileInfo.OpenWrite())
                    {
                        await streamer.CopyToAsync(fileStream);
                    }


                }

                return new JsonResult(new
                    {
                        HospByCounty = 0
                    });
            }

        [HttpGet]
        public async Task<IActionResult> ImportFatalitiesByCron()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/Trials/TexasFatalities.xlsx"));

            //var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var client = new HttpClient();
            var response = await client.GetAsync(@"https://dshs.texas.gov/coronavirus/TexasCOVID19DailyCountyFatalityCountData.xlsx");

            using (var streamer = await response.Content.ReadAsStreamAsync())
            {
                var fileInfo = new FileInfo(path);
                using (var fileStream = fileInfo.OpenWrite())
                {
                    await streamer.CopyToAsync(fileStream);
                }


            }

            return new JsonResult(new
            {
                FatalitiesByCounty = 0
            });
        }

        [HttpGet]
        public async Task<IActionResult> FormatFatalitiesByCron()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Trials/TexasFatalities.xlsx"));

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
                    var lstCounties = _context.DeathsByCounty.Select(s => s.County).Distinct().ToList();
                    int nums = lstCounties.Count;
                    int rowCnt = ws.Dimension.End.Row;
                    int colCnt = ws.Dimension.End.Column + 1;
                    DateTime startingDate = new DateTime(2020, 03, 01);
                    List<DeathByCounty> CasesYall = new List<DeathByCounty>();
                    
                    for (var i = 0; i < lstCounties.Count; i++)
                    {
                        for (var j = 4; j < 258; j++)
                        {
                            if (lstCounties[i] == ws.Cells[j, 1].Value.ToString())
                            {
                                for (var k = 38; k < colCnt; k++)
                                {
                                    var fatalitiesByCounty = new DeathByCounty();
                                    fatalitiesByCounty.Date = startingDate.AddDays(k).Date;
                                    fatalitiesByCounty.County = lstCounties[i];
                                    fatalitiesByCounty.Deaths = ws.Cells[j, k].GetValue<int>();
                                    CasesYall.Add(fatalitiesByCounty);

                                }
                            }
                        }
                    }

                    var dallas = CasesYall.Where(x => x.County == "Dallas");
                    // Initialize the record counters
                    // iterate through all rows, skipping the first one
                    return new JsonResult(new
                    {
                        dallas
                    });
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> ImportHospitalizationByCron()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/Trials/TexasHospitalizations.xlsx"));

            var client = new HttpClient();
            var response = await client.GetAsync(@"https://dshs.texas.gov/coronavirus/TexasCOVID-19HospitalizationsOverTimebyTSA.xlsx");

            using (var streamer = await response.Content.ReadAsStreamAsync())
            {
                var fileInfo = new FileInfo(path);
                using (var fileStream = fileInfo.OpenWrite())
                {
                    await streamer.CopyToAsync(fileStream);
                }
            }

            return new JsonResult(new
            {
                HospitalizationsByCounty = 0
            });
        }

        [HttpGet]
        public async Task<IActionResult> FormatHospitalizationsByCron()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Trials/TexasHospitalizations.xlsx"));

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
                    var lstCounties = _context.CountyHospitalizations.Select(s => s.County).Distinct().ToList();
                    int nums = lstCounties.Count;
                    int rowCnt = ws.Dimension.End.Row;
                    int colCnt = ws.Dimension.End.Column + 1;
                    DateTime startingDate = new DateTime(2020, 04, 05);
                    List<HospByCounty> CasesYall = new List<HospByCounty>();

                    for (var i = 0; i < lstCounties.Count; i++)
                    {
                        for (var j = 4; j < 27; j++)
                        {
                            if (lstCounties[i] == ws.Cells[j, 2].Value.ToString())
                            {
                                for (var k = 3; k < colCnt; k++)
                                {
                                    var hosp = new HospByCounty();
                                    hosp.Date = startingDate.AddDays(k).Date;
                                    hosp.County = lstCounties[i];
                                    hosp.Hospitalizations = ws.Cells[j, k].GetValue<int>();
                                    CasesYall.Add(hosp);

                                }
                            }
                        }
                    }

                    var dallas = CasesYall.Where(x => x.County == "Dallas/Ft. Worth");
                    // Initialize the record counters
                    // iterate through all rows, skipping the first one
                    return new JsonResult(new
                    {
                        dallas
                    });
                }

            }
        }


        [HttpGet]
        public async Task<IActionResult> ImportHospitalizationByCounty()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/Covid/Hospitalizations/TexasCOVID19HospitalizationsbyTSA624.xlsx"));

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
                    var lstHostDates = _context.CountyHospitalizations.ToList();

                    // Initialize the record counters
                    var nCounty = 0;
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var dateId = row[nRow, 4].GetValue<DateTime>();

                        if (lstHostDates.Where(d => d.Date == dateId).Count() == 0)
                        {
                            var hosp = new HospByCounty();
                            hosp.Date = row[nRow, 4].GetValue<DateTime>();
                            hosp.County = row[nRow, 1].GetValue<string>();
                            hosp.Hospitalizations = row[nRow, 3].GetValue<int>();

                            // create the hospbycounty entity and fill it with xlsx data 
                            // save the hospbycounty to the db 
                            _context.CountyHospitalizations.Add(hosp);
                            await _context.SaveChangesAsync();

                            //increment the counter 
                            nCounty++;
                        }

                    }

                    return new JsonResult(new
                    {
                        HospByCounty = nCounty
                    });
                }

            }


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
