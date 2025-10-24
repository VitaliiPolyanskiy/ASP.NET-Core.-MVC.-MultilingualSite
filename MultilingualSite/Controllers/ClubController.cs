using Microsoft.AspNetCore.Mvc;
using MultilingualSite.Filters;
using MultilingualSite.Models;
using MultilingualSite.Services;

namespace MultilingualSite.Controllers
{
    [Culture]
    public class ClubController : Controller
    {
        readonly ClubContext cc;
        readonly ILangRead _langRead;

        public ClubController(ClubContext context, ILangRead langRead)
        {
            cc = context;
            _langRead = langRead;
        }

        public ActionResult Index()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View(cc.Clubs);
        }

        [HttpGet]
        public ActionResult CreateClub()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View();
        }

        [HttpPost]
        public ActionResult CreateClub(Club club)
        {
            if (ModelState.IsValid)
            {
                cc.Clubs.Add(club);
                cc.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path") ?? "/Club/Index";

            // Список культур
            List<string> cultures = _langRead.languageList().Select(t => t.ShortName).ToList()!;
            if (!cultures.Contains(lang))
            {
                lang = "uk";
            }

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10); // срок хранения куки - 10 дней
            Response.Cookies.Append("lang", lang, option); // создание куки
            return Redirect(returnUrl);
        }

    }
}
