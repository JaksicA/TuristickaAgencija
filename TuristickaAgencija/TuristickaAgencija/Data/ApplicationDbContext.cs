using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ponuda> Ponude { get; set; }

        public DbSet<Aranzman> Aranzmani { get; set; }
    }
}
