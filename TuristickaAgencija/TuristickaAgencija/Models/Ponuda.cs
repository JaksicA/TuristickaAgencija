using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristickaAgencija.Models
{
    public class Ponuda : BaseModel
    {
        [Required]
        public string Sezona { get; set; }


        [Required]
        [Display(Name ="Pocetak")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime DatumOd { get; set; }


        [Required]
        [Display(Name ="Zavrsetak")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DatumDo { get; set; }
    }
}
