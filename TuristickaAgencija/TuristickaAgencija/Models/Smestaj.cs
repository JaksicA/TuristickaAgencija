using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristickaAgencija.Models
{
    public class Smestaj : BaseModel
    {
        [Required]
        [Range(1,10)]
        public int BrojKreveta { get; set; }

        [Required]
        public string Tip { get; set; }

        [Required]
        public float Cena { get; set; }

        [Display(Name ="Izaberi aranzman")]
        public Guid AranzmanId { get; set; }

        public Aranzman Aranzman { get; set; }

        public ICollection<Prevoz> Prevozs { get; set; } = new List<Prevoz>();
    }
}
