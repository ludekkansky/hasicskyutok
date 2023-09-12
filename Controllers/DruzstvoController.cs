using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hasicskyutok.Models;
using Microsoft.AspNetCore.Authorization;

namespace hasickyutok.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DruzstvoController : Controller
    {
        private readonly hasicskyutokDbContext _context;

        public DruzstvoController(hasicskyutokDbContext context)
        {
            _context = context;
        }

        // GET: Druzstvo
        public async Task<IActionResult> Index()
        {
            var hasicskyutokDbContext = _context.Druzstva.Include(d => d.Kategorie);
            return View(await hasicskyutokDbContext.OrderBy(s=>s.StartovniCislo).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Druzstva == null)
            {
                return NotFound();
            }

            var druzstvo = await _context.Druzstva
                .Include(d => d.Kategorie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (druzstvo == null)
            {
                return NotFound();
            }

            return View(druzstvo);
        }

        public IActionResult Create()
        {
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev");
            var druzstvo = new hasicskyutok.Models.Druzstvo();
            var posledniStartovniCislo = 0;
            if (_context.Druzstva.Any())
            {
                posledniStartovniCislo = _context.Druzstva.Max(s => s.StartovniCislo);
            }
            druzstvo.StartovniCislo = posledniStartovniCislo + 1;
            return View(druzstvo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nazev,StartovniCislo,KategorieID")] Druzstvo druzstvo)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Druzstva.Any(s => s.StartovniCislo == druzstvo.StartovniCislo))
                {
                    _context.Add(druzstvo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("StartovniCislo","Startovní číslo již použil někdo jiný.");
            }
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev", druzstvo.KategorieID);
            return View(druzstvo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Druzstva == null)
            {
                return NotFound();
            }

            var druzstvo = await _context.Druzstva.FindAsync(id);
            if (druzstvo == null)
            {
                return NotFound();
            }
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev", druzstvo.KategorieID);
            return View(druzstvo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nazev,StartovniCislo,KategorieID")] Druzstvo druzstvo)
        {
            if (id != druzstvo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(druzstvo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DruzstvoExists(druzstvo.ID))
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
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev", druzstvo.KategorieID);
            return View(druzstvo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Druzstva == null)
            {
                return NotFound();
            }

            var druzstvo = await _context.Druzstva
                .Include(d => d.Kategorie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (druzstvo == null)
            {
                return NotFound();
            }

            return View(druzstvo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Druzstva == null)
            {
                return Problem("Entity set 'hasicskyutokDbContext.Druzstva'  is null.");
            }
            var druzstvo = await _context.Druzstva.FindAsync(id);
            if (druzstvo != null)
            {
                _context.Druzstva.Remove(druzstvo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DruzstvoExists(int id)
        {
            return _context.Druzstva.Any(e => e.ID == id);
        }
    }
}
