using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes_App_C_.Data;
using Notes_App_C_.Models;

namespace Notes_App_C_.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Zobrazení seznamu poznámek - seřazeno od nejnovějších 
        public async Task<IActionResult> Index(bool onlyImportant = false)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var query = _context.Notes.Where(n => n.UserId == userId);

            if (onlyImportant)
            {
                query = query.Where(n => n.IsImportant);
            }

            var notes = await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
            ViewBag.OnlyImportant = onlyImportant;
            return View(notes);
        }

        // Stránka pro vytvoření poznámky
        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            return View();
        }

        // Uložení poznámky
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // KRITICKÁ OPRAVA: Odstraníme navigaci na uživatele z validace. 
            // ASP.NET se jinak snaží validovat celého "Usera", kterého ale ve formuláři neposíláme.
            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                note.UserId = userId.Value; // Propojení s přihlášeným uživatelem [cite: 25]
                note.CreatedAt = DateTime.UtcNow; // Automatické přiřazení času [cite: 26]

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Pokud validace selže, vrátíme uživatele zpět k formuláři s chybami
            return View(note);
        }

        // Smazání poznámky 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Najdeme poznámku a zároveň ověříme, že patří přihlášenému uživateli (bezpečnost)
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleImportant(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                note.IsImportant = !note.IsImportant;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}