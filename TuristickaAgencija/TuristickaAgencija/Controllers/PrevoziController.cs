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
    public class PrevoziController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrevoziController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prevozi
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prevozi.Include(p => p.Aranzman);
            CreatePrevozViewModel viewModel = new CreatePrevozViewModel
            {
                Prevozs = await applicationDbContext.ToListAsync()
            };
            return View(viewModel);
        }

        // GET: PrevozUaranzmanu

        public async Task<IActionResult> PrevozUaranzmanu (Guid aranzmanId)
        {
            var applicationDbContext = _context.Prevozi.Include(p => p.Aranzman).Where(a => a.AranzmanId == aranzmanId);
            CreatePrevozViewModel viewModel = new CreatePrevozViewModel
            {
                Prevozs = await applicationDbContext.ToListAsync(),
                AranzmanId = aranzmanId
            };
            return View("Index",viewModel);
        }

        // GET: Prevozi/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prevoz = await _context.Prevozi
                .Include(p => p.Aranzman)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prevoz == null)
            {
                return NotFound();
            }

            return View(prevoz);
        }

        // GET: Prevozi/Create
        public IActionResult Create(Guid aranzmanId)
        {
            ViewData["AranzmanId"] = aranzmanId;
            return View();
        }

        // POST: Prevozi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VrstaPrevoza,Cena,AranzmanId,Id")] Prevoz prevoz)
        {
            if (ModelState.IsValid)
            {
                prevoz.Id = Guid.NewGuid();
                _context.Add(prevoz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PrevozUaranzmanu),new { aranzmanId = prevoz.AranzmanId });
            }
            return View(prevoz);
        }

        // GET: Prevozi/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prevoz = await _context.Prevozi.FindAsync(id);
            if (prevoz == null)
            {
                return NotFound();
            }
            return View(prevoz);
        }

        // POST: Prevozi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VrstaPrevoza,Cena,AranzmanId,Id")] Prevoz prevoz)
        {
            if (id != prevoz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prevoz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrevozExists(prevoz.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PrevozUaranzmanu),new { aranzmanId = prevoz.AranzmanId });
            }
            return View(prevoz);
        }

        // GET: Prevozi/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prevoz = await _context.Prevozi
                .Include(p => p.Aranzman)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prevoz == null)
            {
                return NotFound();
            }

            return View(prevoz);
        }

        // POST: Prevozi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var prevoz = await _context.Prevozi.FindAsync(id);
            _context.Prevozi.Remove(prevoz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrevozExists(Guid id)
        {
            return _context.Prevozi.Any(e => e.Id == id);
        }
    }
}
