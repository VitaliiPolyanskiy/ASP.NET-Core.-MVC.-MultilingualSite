using Microsoft.EntityFrameworkCore;
using MultilingualSite.Models;
using MultilingualSite.Services;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок підключення з файлу конфігурації
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Додаємо контекст ApplicationContext як сервіс у додаток
builder.Services.AddDbContext<ClubContext>(options => options.UseSqlServer(connection));

// Усі сесії працюють поверх об'єкта IDistributedCache, і ASP.NET Core 
// надає вбудовану реалізацію IDistributedCache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Тривалість сеансу (тайм-аут завершення сеансу)
    options.Cookie.Name = "Session"; // Кожна сесія має свій ідентифікатор, який зберігається в куках
});

builder.Services.AddScoped<ILangRead, ReadLangServices>();

// Додаємо сервіси MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles(); // Обробляє запити до файлів у папці wwwroot
app.UseSession();     // Додаємо middleware-компонент для роботи з сесіями

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Club}/{action=Index}/{id?}");

app.Run();