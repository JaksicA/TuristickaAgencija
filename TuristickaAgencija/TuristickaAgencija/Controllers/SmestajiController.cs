using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TuristickaAgencija.Data;
using TuristickaAgencija.Models;
using TuristickaAgencija.ViewModels;

namespace TuristickaAgencija.Controllers
{
    public class SmestajiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmestajiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Smestaji
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Smestaji.Include(s => s.Aranzman);
            CreateSmestajViewModel viewModel = new CreateSmestajViewModel
            {
                Smestajs = await applicationDbContext.ToListAsync()
            };
            return View(viewModel);
        }

        // GET: SmestajUAranzmanu
        public async Task<IActionResult> SmestajUaranzmanu(Guid aranzmanId)
        {
            var applicationDbContext = _context.Smestaji.Include(s => s.Aranzman).Where(a => a.AranzmanId == aranzmanId);
            CreateSmestajViewModel viewModel = new CreateSmestajViewModel
            {
                Smestajs = await applicationDbContext.ToListAsync(),
                AranzmanId = aranzmanId
            };
            return View("Index", viewModel);
        }

        // GET: Smestaji/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smestaj = await _context.Smestaji
                .Include(s => s.Aranzman)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smestaj == null)
            {
                return NotFound();
            }

            return View(smestaj);
        }

        // GET: Smestaji/Create
        public IActionResult Create(Guid aranzmanId)
        {
            ViewData["AranzmanId"] = aranzmanId;    
            return View();
        }

        // POST: Smestaji/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrojKreveta,Tip,Cena,AranzmanId,Id")] Smestaj smestaj)
        {
            if (ModelState.IsValid)
            {
                smestaj.Id = Guid.NewGuid();
                _context.Add(smestaj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SmestajUaranzmanu),new { aranzmanId = smestaj.AranzmanId });
            }
            return View(smestaj);
        }

        // GET: Smestaji/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smestaj = await _context.Smestaji.FindAsync(id);
            if (smestaj == null)
            {
                return NotFound();
            }
            return View(smestaj);
        }

        // POST: Smestaji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BrojKreveta,Tip,Cena,AranzmanId,Id")] Smestaj smestaj)
        {
            if (id != smestaj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(smestaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SmestajExists(smestaj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SmestajUaranzmanu), new { aranzmanId = smestaj.AranzmanId });
            }
            return View(smestaj);
        }

        // GET: Smestaji/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smestaj = await _context.Smestaji
                .Include(s => s.Aranzman)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smestaj == null)
            {
                return NotFound();
            }

            return View(smestaj);
        }

        // POST: Smestaji/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var smestaj = await _context.Smestaji.FindAsync(id);
            _context.Smestaji.Remove(smestaj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SmestajUaranzmanu),new {aranzmanId = smestaj.AranzmanId });
        }

        private bool SmestajExists(Guid id)
        {
            return _context.Smestaji.Any(e => e.Id == id);
        }
    }
}
