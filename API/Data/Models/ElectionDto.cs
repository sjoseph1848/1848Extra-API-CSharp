using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class ElectionDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string DemCandidateName { get; set; }
        public string RepCandidateName { get; set; }
        public int RPct { get; set; }
        public int DPct { get; set; }
    }
}
