using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TuristickaAgencija.Data;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Controllers
{
    public class AranzmaniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AranzmaniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aranzmani
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Aranzmani.Include(a => a.Ponuda);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Aranzmani/Ponuda

        public async Task<IActionResult> AranzmanUponudi(Guid ponudaId)
        {
            var applicationDbContext = _context.Aranzmani.Include(a => a.Ponuda).Where(a => a.PonudaId == ponudaId);
            return View("Index",await applicationDbContext.ToListAsync());
        }

        // GET: Aranzmani/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aranzman = await _context.Aranzmani
                .Include(a => a.Ponuda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aranzman == null)
            {
                return NotFound();
            }

            return View(aranzman);
        }

        // GET: Aranzmani/Create
        public IActionResult Create()
        {
            ViewData["PonudaId"] = new SelectList(_context.Ponude, "Id", "Sezona");
            return View();
        }

        // POST: Aranzmani/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrojDana,Mesto,Popust,PonudaId,Id")] Aranzman aranzman)
        {
            if (ModelState.IsValid)
            {
                aranzman.Id = Guid.NewGuid();
                _context.Add(aranzman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PonudaId"] = new SelectList(_context.Ponude, "Id", "Sezona", aranzman.PonudaId);
            return View(aranzman);
        }

        // GET: Aranzmani/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aranzman = await _context.Aranzmani.FindAsync(id);
            if (aranzman == null)
            {
                return NotFound();
            }
            ViewData["PonudaId"] = new SelectList(_context.Ponude, "Id", "Sezona", aranzman.PonudaId);
            return View(aranzman);
        }

        // POST: Aranzmani/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BrojDana,Mesto,Popust,PonudaId,Id")] Aranzman aranzman)
        {
            if (id != aranzman.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aranzman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AranzmanExists(aranzman.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PonudaId"] = new SelectList(_context.Ponude, "Id", "Sezona", aranzman.PonudaId);
            return View(aranzman);
        }

        // GET: Aranzmani/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aranzman = await _context.Aranzmani
                .Include(a => a.Ponuda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aranzman == null)
            {
                return NotFound();
            }

            return View(aranzman);
        }

        // POST: Aranzmani/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var aranzman = await _context.Aranzmani.FindAsync(id);
            _context.Aranzmani.Remove(aranzman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AranzmanExists(Guid id)
        {
            return _context.Aranzmani.Any(e => e.Id == id);
        }
    }
}
