using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristickaAgencija.Models
{
    public class Rezervacija : BaseModel
    {
        [Required]
        public string  UserId { get; set; }

        public IdentityUser User { get; set; }

        public Guid SmestajId { get; set; }

        [Display(Name ="Izaberite prevoz")]
        public Guid PrevozId { get; set; }

        public Smestaj Smestaj { get; set; }
        public Prevoz Prevoz { get; set; }
    }
}
