using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TuristickaAgencija.Data;
using TuristickaAgencija.Models;
using TuristickaAgencija.Strings;

namespace TuristickaAgencija.Controllers
{
    [Authorize(Roles =RoleNames.AdminIkorisnik)]
    
    public class RezervacijasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervacijas
        public async Task<IActionResult> Index()
        {
            List<Rezervacija> rezervacijas;

            if (User.IsInRole(RoleNames.Admin))
            {
                rezervacijas = await GetRezervacijas();
            }
            else
            {
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                rezervacijas = await GetRezervacijasByUserId(currentUserId);
            }
            return View(rezervacijas);
        }

        // GET: Rezervacijas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Prevoz)
                .Include(r => r.Smestaj)
                .ThenInclude(s => s.Aranzman)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacijas/Create
        [Authorize(Roles=RoleNames.Korisnik)]
        public IActionResult Create(Guid smestajId)
        {
            var smestaj = _context
                .Smestaji
                .Include(s => s.Prevozs)
                .FirstOrDefault(s => s.Id == smestajId);

            List<SelectListItem> listaPrevoza = new List<SelectListItem>();

            listaPrevoza.AddRange(smestaj.Prevozs.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = string.Join(" - ", p.VrstaPrevoza, p.Cena + "e")
            }));

            ViewData["PrevozId"] = listaPrevoza;
            return View();
        }

        // POST: Rezervacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=RoleNames.Korisnik)]
        public async Task<IActionResult> Create([Bind("UserId,SmestajId,PrevozId,Id")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                rezervacija.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                rezervacija.Id = Guid.NewGuid();
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var smestaj = _context
               .Smestaji
               .Include(s => s.Prevozs)
               .FirstOrDefault(s => s.Id == rezervacija.SmestajId);

            List<SelectListItem> listaPrevoza = new List<SelectListItem>();

            listaPrevoza.AddRange(smestaj.Prevozs.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = string.Join(" - ", p.VrstaPrevoza, p.Cena + "e")
            }));

            ViewData["PrevozId"] = listaPrevoza;
            return View(rezervacija);
        }



        // GET: Rezervacijas/Delete/5
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rezervacija = await _context.Rezervacije
                .Include(r => r.Prevoz)
                .Include(r => r.Smestaj)
                .ThenInclude(s => s.Aranzman)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (rezervacija.UserId != currentUserId)
            {
                return Forbid();
            }

            return View(rezervacija);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=RoleNames.AdminIkorisnik)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija.UserId != currentUserId)
            {
                return Forbid();
            }
            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(Guid id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }

        private Task<List<Rezervacija>> GetRezervacijas()
        {
            var rezervacijas =  _context.Rezervacije
               .Include(r => r.Prevoz)
               .Include(r => r.Smestaj)
               .ThenInclude(s => s.Aranzman)
               .Include(r => r.User);
            return rezervacijas.ToListAsync();
        }

        private Task<List<Rezervacija>> GetRezervacijasByUserId(string userId)
        {
            var rezervacijas = _context.Rezervacije
               .Include(r => r.Prevoz)
               .Include(r => r.Smestaj)
               .ThenInclude(s => s.Aranzman)
               .Include(r => r.User)
               .Where(r => r.UserId == userId);
            return rezervacijas.ToListAsync();
        }
    }
}
