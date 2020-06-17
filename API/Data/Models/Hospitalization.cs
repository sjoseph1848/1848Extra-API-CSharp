using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Models
{
    public class Hospitalization
    {
        public Hospitalization()
        {

        }

        [Key]
        [Required]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Total { get; set; }

        public int Amarillo { get; set; }

        public int Lubbock { get; set; }
        public int Wichita { get; set; }
        public int Abilene { get; set; }

        public int Dallas { get; set; }

        public int Paris { get; set; }

        public int Tyler { get; set; }

        public int Lufkin { get; set; }

        public int ElPaso { get; set; }

        public int MidlanOdessa { get; set; }

        public int SanAngelo { get; set; }

        public int BeltonKilleen { get; set; }

        public int Waco { get; set; }

        public int BryanCollegStation { get; set; }

        public int Austin { get; set; }

        public int SanAntonio { get; set; }
        public int Houston { get; set; }
        public int Galveston { get; set; }

        public int Victoria { get; set; }
        public int Laredo { get; set; }
        public int CorpusChristi { get; set; }
        public int RioGrandeValley { get; set; }

    }
}
