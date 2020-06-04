using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class ElectionDto
    {
        public int QuestionId { get; set; }
        public string DemCandidateName { get; set; }
        public string RepCandidateName { get; set; }
        public string State { get; set; }
        public string FteGrade { get; set; }
        public int SampleSize { get; set; }
        public string Methodology { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Partisan { get; set; }
        public int RPct { get; set; }
        public int DPct { get; set; }
    }
}
