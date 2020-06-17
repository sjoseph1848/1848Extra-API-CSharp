using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class CasesByCounty
    {
        public CasesByCounty()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        public string County { get; set; }

        public int Cases { get; set; }

        public DateTime Date { get; set; }
    }
}
