using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.ViewModels
{
    public class CreatePrevozViewModel
    {
        public Guid AranzmanId { get; set; }
        public List<Prevoz> Prevozs { get; set; }

    }
}
