using Microsoft.AspNetCore.Mvc.Filters;
using MultilingualSite.Services;
using System.Globalization;

namespace MultilingualSite.Filters
{
    // модификаторы доступа для каждого файла ресурсов установлены в public. 
    // У файлов ресурсов значение свойства «Custom Tool» равно «PublicResXFileCodeGenerator» - инструмент создания ресурсов.
    // Иначе файлы ресурсов не будут скомпилированы и доступны.
    // Build Action - Embedded Resource
    // Custom Tool Namespace - Resources.

    public class CultureAttribute : Attribute, IActionFilter
    {
       
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
          
        }

        // Фильтр действий, который будет срабатывать при обращении к действиям контроллера и производить локализацию.
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string? cultureName = null;
            // Получаем куки из контекста, которые могут содержать установленную культуру
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null)
                cultureName = cultureCookie;
            else
            {
                cultureName = "uk";// Получаем объект запроса
            }

            // Список культур
            List<string> cultures = filterContext.HttpContext.RequestServices.GetRequiredService<ILangRead>()
                                    .languageList().Select(t => t.ShortName).ToList()!;
            if (!cultures.Contains(cultureName))
            {
                cultureName = "uk";
            }
            // CultureInfo.CreateSpecificCulture создает объект CultureInfo, 
            // который представляет определенный язык и региональные параметры, 
            // соответствующие заданному имени. Функциональность этого объекта зависит от культурного контекста,
            // например, форматирование дат, времени, чисел, валюты, работа с календарем. 

            // При запуске приложения каждый поток в .NET определяет два объекта типа CultureInfo:
            // CurrentCulture - текущую языковую культуру
            // CultureInfo.CurrentUICulture - языковую культуру для пользовательского интерфейса.
            // ASP.NET Core использует эти свойства для рендеринга значений, которые зависят от настройки культуры.
            // Например, в зависимости от культуры может меняться отображение даты и времени.

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);

            // После этого для локализации система будет выбирать нужный файл ресурсов.
        }
    }
}