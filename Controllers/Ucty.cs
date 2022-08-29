using Microsoft.AspNetCore.Mvc;
using hasicskyutok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace hasickyutok.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UctyController : Controller
    {
        private readonly hasicskyutokDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UctyController(hasicskyutokDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var uzivatele = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            if (!roles.Any(s => s.Name == "Admin")) return NotFound("Role admin neni definovana.");

            var seznam = uzivatele.Select(s => new hasicskyutok.ViewModel.PrehledUcet()
            {
                Id = s.Id,
                Email = s.Email,
                Potvrzen = s.EmailConfirmed,
                Skupiny = string.Join(",", _userManager.GetRolesAsync(s).Result.ToArray())
            });

            foreach (var s in seznam)
                Console.Write($"u:{s.ToString()}");

            return View(seznam);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            // if (string.IsNullOrEmpty(Email))
            // {
            //     return NotFound("predany parametr je prazdny");
            // }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Kategorie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
