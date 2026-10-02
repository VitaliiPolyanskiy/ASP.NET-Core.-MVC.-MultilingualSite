using Microsoft.AspNetCore.Mvc.Filters;
using MultilingualSite.Services;
using System.Globalization;

namespace MultilingualSite.Filters
{
    // Модифікатори доступу для кожного файлу ресурсів встановлені в public. 
    // У файлів ресурсів значення властивості «Custom Tool» дорівнює «PublicResXFileCodeGenerator» — інструмент створення ресурсів.
    // Інакше файли ресурсів не будуть скомпільовані та доступні.
    // Build Action - Embedded Resource
    // Custom Tool Namespace - Resources.
    public class CultureAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        // Фільтр дій, який спрацьовує при зверненні до дій контролера і виконує локалізацію.
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Отримуємо cookie з контексту, які можуть містити встановлену культуру
            var cultureName = filterContext.HttpContext.Request.Cookies["lang"] ?? "uk";

            // Отримуємо список доступних культур через DI
            var langService = filterContext.HttpContext.RequestServices.GetRequiredService<ILangRead>();
            List<string> cultures = langService.GetLanguageList().Select(t => t.ShortName).ToList()!;

            if (!cultures.Contains(cultureName))
            {
                cultureName = "uk";
            }

            // CultureInfo.CreateSpecificCulture створює об'єкт CultureInfo, 
            // який представляє певну мову та регіональні параметри.
            // Під час запуску програми кожен потік у .NET визначає два об'єкти типу CultureInfo:
            // CurrentCulture - поточну мовну культуру
            // CurrentUICulture - мовну культуру для інтерфейсу користувача.
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}