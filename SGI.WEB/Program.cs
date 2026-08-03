using Microsoft.AspNetCore.Authentication.Cookies;
using SGIbiblioteca.Domain.Interfaces;
using SGI.Application.Services;
using SGI.WEB.Services;

namespace SGI.WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ILoggerService, LoggerService>();

            builder.Services.AddHttpClient<IUsuarioApiService, UsuarioApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });
            builder.Services.AddHttpClient<ILibroApiService, LibroApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });
            builder.Services.AddHttpClient<IDevolucionApiService, DevolucionApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });
            builder.Services.AddHttpClient<IPenalizacionApiService, PenalizacionApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });
            builder.Services.AddHttpClient<IPrestamoApiService, PrestamoApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });
            builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7289/api/");
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied"; 
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");
            app.Run();
        }
    }
}
