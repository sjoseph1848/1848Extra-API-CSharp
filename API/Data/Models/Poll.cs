using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class Poll
    {
        public Poll()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int PollId { get; set; }

        public string State { get; set; }

        public int PollsterId { get; set; }

        public int? SponsorIds { get; set; }

        public string DisplayName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual List<PresPoll> PresPoll { get; set; }
        
    }
}
