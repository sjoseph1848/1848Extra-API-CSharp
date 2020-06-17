using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class HospByCounty
    {
        public HospByCounty()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        public string County { get; set; }

        public int Hospitalizations { get; set; }

        public DateTime Date { get; set; }

    }
}
