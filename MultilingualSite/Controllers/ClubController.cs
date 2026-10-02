using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultilingualSite.Filters;
using MultilingualSite.Models;
using MultilingualSite.Services;

namespace MultilingualSite.Controllers
{
    [Culture]
    public class ClubController : Controller
    {
        private readonly ClubContext _context;
        private readonly ILangRead _langRead;

        public ClubController(ClubContext context, ILangRead langRead)
        {
            _context = context;
            _langRead = langRead;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View(await _context.Clubs.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateClub()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Захист від підробки міжсайтових запитів (CSRF)
        public async Task<IActionResult> CreateClub(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();

                // Використання nameof() замість жорстко закодованих рядків
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        public IActionResult ChangeCulture(string lang)
        {
            string returnUrl = HttpContext.Session.GetString("path") ?? "/Club/Index";

            List<string> cultures = _langRead.GetLanguageList().Select(t => t.ShortName).ToList()!;

            if (!cultures.Contains(lang))
            {
                lang = "uk";
            }

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(10) // термін зберігання cookie - 10 днів
            };

            Response.Cookies.Append("lang", lang, options); // створення cookie

            return Redirect(returnUrl);
        }
    }
}