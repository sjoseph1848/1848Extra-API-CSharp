using System;
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

        /// <summary>
        /// Question Id (foreign key)
        /// </summary>
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public virtual Poll Poll { get; set; }
        public int PollId { get; set; }
        public string Stage { get; set; }
        public int RaceId { get; set; }
        public string State { get; set; }
        public string CandidateName { get; set; }
        public string CandidateParty { get; set; }
        public int pct { get; set; }

    }
}
