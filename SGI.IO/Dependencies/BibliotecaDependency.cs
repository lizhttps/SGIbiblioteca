using Microsoft.Extensions.DependencyInjection;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.Repositorios;
using SGI.Application.Interfaces;
using SGI.Application.Service;
using SGI.Application.Services;
namespace SGI.IOC.Dependencies // hola
{
    public static class BibliotecaDependency
    {
        public static void AddBibliotecaDependency(this IServiceCollection service)
        {
            service.AddScoped<ILoggerService, LoggerService>();

            service.AddScoped<ILibroRepository, LibroRepository>();
            service.AddScoped<IUsuarioRepository, UsuarioRepository>();
            service.AddScoped<INotificacionRepository, NotificacionRepository>();
            service.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            service.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            service.AddScoped<IDevolucionRepository, DevolucionRepository>();
            service.AddScoped<IPrestamoRepository, PrestamoRepository>();

            service.AddTransient<ILibroService, LibroService>();
            service.AddTransient<IUsuarioService, UsuarioService>();
            service.AddTransient<INotificacionService, NotificacionService>();
            service.AddTransient<IAuditoriaService, AuditoriaService>();
            service.AddTransient<IPenalizacionService, PenalizacionService>();
            service.AddTransient<IDevolucionService, DevolucionService>();
            service.AddTransient<IPrestamoService, PrestamoService>();
        }
    }
}