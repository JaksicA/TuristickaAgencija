using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.ViewModels
{
    public class CreatePrevozViewModel
    {
        public Guid SmestajId { get; set; }
        public Guid PrevozId { get; set; }
        public List<SelectListItem> DostupniPrevoz { get; set; } = new List<SelectListItem>();

    }
}
