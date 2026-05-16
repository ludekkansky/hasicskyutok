using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HasicskyUtok.Models;
using Microsoft.AspNetCore.Authorization;

namespace HasicskyUtok.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StafetaController : Controller
    {
        private readonly HasicskyUtokDbContext _context;

        public StafetaController(HasicskyUtokDbContext context)
        {
            _context = context;
        }

        // GET: Kategorie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stafeta.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stafeta == null)
            {
                return NotFound();
            }

            var stafeta = await _context.Stafeta.FirstOrDefaultAsync(m => m.ID == id);
            if (stafeta == null)
            {
                return NotFound();
            }

            return View(stafeta);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategorie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stafeta stafeta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stafeta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stafeta);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stafeta == null)
            {
                return NotFound();
            }

            var stafeta = await _context.Stafeta.FindAsync(id);
            if (stafeta == null)
            {
                return NotFound();
            }
            return View(stafeta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stafeta stafeta)
        {
            if (id != stafeta.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stafeta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StafetaExists(stafeta.ID))
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
            return View(stafeta);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stafeta == null)
            {
                return NotFound();
            }

            var stafeta = await _context.Stafeta.FirstOrDefaultAsync(m => m.ID == id);
            if (stafeta == null)
            {
                return NotFound();
            }

            return View(stafeta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stafeta == null)
            {
                return Problem("Entity set 'hasickyutokDbContext.Stafeta'  is null.");
            }
            var stafeta = await _context.Stafeta.FindAsync(id);
            if (stafeta != null)
            {
                _context.Stafeta.Remove(stafeta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StafetaExists(int id)
        {
            return _context.Stafeta.Any(e => e.ID == id);
        }
    }
}
