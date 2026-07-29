using SGI.Application.Interfaces;
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
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}