using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristickaAgencija.Models
{
    public class Aranzman : BaseModel
    {
        [Required]
        [Display(Name ="Broj dana")]
        [Range(1,30)]
        public int BrojDana { get; set; }


        [Required]
        public string Mesto { get; set; }


        [Range(0,100)]
        public float Popust { get; set; }


        [Display(Name ="Izaberi ponudu")]
        public Guid PonudaId { get; set; }


        public Ponuda Ponuda { get; set; }
    }
}
