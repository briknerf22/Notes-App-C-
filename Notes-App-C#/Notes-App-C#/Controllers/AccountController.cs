using BCrypt.Net; // Pro hashování hese
using Microsoft.AspNetCore.Mvc;
using Notes_App_C_.Data;
using Notes_App_C_.Models;
using Microsoft.EntityFrameworkCore;   

namespace Notes_App_C_.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Najdeme uživatele podle jména
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                // 2. Ověříme, zda uživatel existuje a zda zadané heslo odpovídá hashi v DB
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Neplatné jméno nebo heslo.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Vymaže session a odhlásí uživatele
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kontrola, zda uživatel už neexistuje
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Uživatel s tímto jménem již existuje.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // Tady se heslo "rozmixuje"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteAccount() => View();

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = await _context.Users.Include(u => u.Notes).FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _context.Users.Remove(user); // Smaže uživatele i jeho poznámky (díky kaskádě v DB)
                await _context.SaveChangesAsync();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nesprávné heslo.");
            return View();
        }
    }
}