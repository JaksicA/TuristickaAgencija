using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PonudeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PonudeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ponude
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ponude.ToListAsync());
        }

        // GET: Ponude/Details/5
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ponuda = await _context.Ponude
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ponuda == null)
            {
                return NotFound();
            }

            return View(ponuda);
        }

        // GET: Ponude/Create
        [Authorize(Roles = RoleNames.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ponude/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Create([Bind("Sezona,DatumOd,DatumDo,Id")] Ponuda ponuda)
        {            
            if (ModelState.IsValid)
            {
                ponuda.Id = Guid.NewGuid();
                _context.Add(ponuda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ponuda);
        }

        // GET: Ponude/Edit/5
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ponuda = await _context.Ponude.FindAsync(id);
            if (ponuda == null)
            {
                return NotFound();
            }
            return View(ponuda);
        }

        // POST: Ponude/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Edit(Guid id, [Bind("Sezona,DatumOd,DatumDo,Id")] Ponuda ponuda)
        {
            if (id != ponuda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ponuda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PonudaExists(ponuda.Id))
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
            return View(ponuda);
        }

        // GET: Ponude/Delete/5
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ponuda = await _context.Ponude
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ponuda == null)
            {
                return NotFound();
            }

            return View(ponuda);
        }

        // POST: Ponude/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ponuda = await _context.Ponude.FindAsync(id);
            _context.Ponude.Remove(ponuda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PonudaExists(Guid id)
        {
            return _context.Ponude.Any(e => e.Id == id);
        }
    }
}
