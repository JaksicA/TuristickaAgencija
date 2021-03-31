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
using TuristickaAgencija.ViewModels;

namespace TuristickaAgencija.Controllers
{
    [Authorize(Roles =RoleNames.AdminIkorisnik)]
    public class SmestajiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmestajiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Smestaji
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smestaj = await _context.Smestaji
                .Include(s => s.Aranzman)
                .Include(p => p.Prevozs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smestaj == null)
            {
                return NotFound();
            }

            return View(smestaj);
        }

        // GET: Smestaji/Create
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
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
        [Authorize(Roles = RoleNames.Admin)]
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


        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DodajPrevoz(Guid smestajId)
        {
            bool exist = SmestajExists(smestajId);
            if (!exist)
            {
                return NotFound();
            }
            else
            {
                var prevozi =await _context.Prevozi.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = string.Join(" - ", a.VrstaPrevoza, a.Cena + "e")
                }).ToListAsync();

                CreatePrevozViewModel viewModel = new CreatePrevozViewModel
                {
                    DostupniPrevoz = prevozi,
                    SmestajId = smestajId
                };
                return View("AddAvailableTransport", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DodajPrevoz ([Bind("PrevozId,SmestajId")]CreatePrevozViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                var prevozi = await GetPrevoz();

                viewModel.DostupniPrevoz.AddRange(prevozi);

                return View("AddAvailableTransport", viewModel);
            }

            var smestaj = _context.Smestaji.Include(p => p.Prevozs).FirstOrDefault(s => s.Id == viewModel.SmestajId);
            var prevoz = _context.Prevozi.FirstOrDefault(p => p.Id == viewModel.PrevozId);
            if (smestaj.Prevozs.Any(p => p.Id == viewModel.PrevozId))
            {
                var prevozi = await GetPrevoz();

                viewModel.DostupniPrevoz.AddRange(prevozi);

                ModelState.AddModelError("Greska", "Vec postoji!");

                return View("AddAvailableTransport", viewModel);
            }
            smestaj.Prevozs.Add(prevoz);


            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = viewModel.SmestajId });
        }

        public async Task<List<SelectListItem>> GetPrevoz()
        {
            return  await _context.Prevozi.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Join(" - ", a.VrstaPrevoza, a.Cena + "e")
            }).ToListAsync();
        }

        public async Task<IActionResult> ObrisiPrevoz (Guid id , Guid prevozId)
        {
            var smestaj = _context.Smestaji.Include(p => p.Prevozs).FirstOrDefault(s => s.Id == id);
            var prevoz = _context.Prevozi.FirstOrDefault(p => p.Id == prevozId);

            smestaj.Prevozs.Remove(prevoz);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = id });
        }
    }
}
