using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristickaAgencija.Models
{
    public class Prevoz : BaseModel
    {
        [Required]
        [Display(Name ="Vrsta prevoza")]
        public string VrstaPrevoza { get; set; }

        public float Cena { get; set; }

        public ICollection<Smestaj> Smestajs { get; set; } = new List<Smestaj>();
    }
}
