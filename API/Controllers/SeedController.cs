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
        public async Task<IActionResult> ImportDeathByCounty()
        {
            var path = Path.Combine(
              _env.ContentRootPath,
              String.Format("Data/Source/Covid/Death/TexasCOVID19Deaths620.xlsx"));

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
              String.Format("Data/Source/Covid/Cases/TexasCOVID19CaseCountDatabyCounty620.xlsx"));

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
        public async Task<IActionResult> ImportHospitalizationByCounty()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/Covid/Hospitalizations/TexasCOVID19HospitalizationsbyTSA620.xlsx"));

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
        public async Task<IActionResult> ImportHospitalization()
        {
            var path = Path.Combine(
               _env.ContentRootPath,
               String.Format("Data/Source/TexasHospitalizations612.xlsx"));

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
                    var lstHostDates = _context.Hospitalizations.ToList();

                    // Initialize the record counters
                    var nHosp = 0; 
                    // iterate through all rows, skipping the first one
                    for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var dateId = row[nRow, 1].GetValue<DateTime>();

                        if (lstHostDates.Where(d => d.Date == dateId).Count() == 0)
                        {
                            var hosp = new Hospitalization();
                            hosp.Date = row[nRow, 1].GetValue<DateTime>();
                            hosp.Amarillo = row[nRow, 2].GetValue<int>();
                            hosp.Lubbock = row[nRow, 3].GetValue<int>();
                            hosp.Wichita = row[nRow, 4].GetValue<int>();
                            hosp.Abilene = row[nRow, 5].GetValue<int>();
                            hosp.Dallas = row[nRow, 6].GetValue<int>();
                            hosp.Paris = row[nRow, 7].GetValue<int>();
                            hosp.Tyler = row[nRow, 8].GetValue<int>();
                            hosp.Lufkin = row[nRow, 9].GetValue<int>();
                            hosp.ElPaso = row[nRow, 10].GetValue<int>();
                            hosp.MidlanOdessa = row[nRow, 11].GetValue<int>();
                            hosp.SanAngelo = row[nRow, 12].GetValue<int>();
                            hosp.BeltonKilleen = row[nRow, 13].GetValue<int>();
                            hosp.Waco = row[nRow, 14].GetValue<int>();
                            hosp.BryanCollegStation = row[nRow, 15].GetValue<int>();
                            hosp.Austin = row[nRow, 16].GetValue<int>();
                            hosp.SanAntonio = row[nRow, 17].GetValue<int>();
                            hosp.Houston = row[nRow, 18].GetValue<int>();
                            hosp.Galveston = row[nRow, 19].GetValue<int>();
                            hosp.Victoria = row[nRow, 20].GetValue<int>();
                            hosp.Laredo = row[nRow, 21].GetValue<int>();
                            hosp.CorpusChristi = row[nRow, 22].GetValue<int>();
                            hosp.RioGrandeValley = row[nRow, 23].GetValue<int>();
                            hosp.Total = row[nRow, 24].GetValue<int>();

                            // create the prespoll entity and fill it with xlsx data 
                            // save the pres poll to the db 
                            _context.Hospitalizations.Add(hosp);
                            await _context.SaveChangesAsync();

                            //increment the counter 
                            nHosp++;
                        }

                    }

                    return new JsonResult(new
                    {
                        Hospitalization = nHosp
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
