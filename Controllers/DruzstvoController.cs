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
            return View(await hasicskyutokDbContext.ToListAsync());
        }

        // GET: Druzstvo/Details/5
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

        // GET: Druzstvo/Create
        public IActionResult Create()
        {
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev");
            return View();
        }

        // POST: Druzstvo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nazev,StartovniCislo,KategorieID")] Druzstvo druzstvo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(druzstvo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategorieID"] = new SelectList(_context.Kategorie, "ID", "Nazev", druzstvo.KategorieID);
            return View(druzstvo);
        }

        // GET: Druzstvo/Edit/5
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

        // POST: Druzstvo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Druzstvo/Delete/5
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

        // POST: Druzstvo/Delete/5
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
