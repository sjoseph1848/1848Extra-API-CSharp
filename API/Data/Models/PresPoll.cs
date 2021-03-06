﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class PresPoll
    {
        public PresPoll()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int PollId { get; set; }
        public string State { get; set; }
        public long PollsterId { get; set; }
        public long SponsorId { get; set; }
        public long PollsterRatingId { get; set; }
        public string FteGrade { get; set; }
        public int SampleSize { get; set; }
        public string Methodology { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Partisan { get; set; }
        public int RaceId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateParty { get; set; }
        public int Pct { get; set; }

    }
}
