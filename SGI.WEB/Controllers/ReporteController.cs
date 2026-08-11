using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGI.WEB.Models.Reporte;
using SGI.WEB.Services.Devolucion;
using SGI.WEB.Services.Libro;
using SGI.WEB.Services.Prestamo;
using SGIbiblioteca.Domain.Interfaces;

namespace SGI.WEB.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class ReporteController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly ILibroApiService _libroApiService;
        private readonly ILoggerService _loggerService;

        public ReporteController(
            IPrestamoApiService prestamoApiService,
            ILibroApiService libroApiService,
            ILoggerService loggerService)
        {
            _prestamoApiService = prestamoApiService;
            _libroApiService = libroApiService;
            _loggerService = loggerService;
        }

        // GET: Reporte
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reporte/LibrosMasPrestados
        public async Task<IActionResult> LibrosMasPrestados()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();
                var lista = new List<LibroMasPrestadoViewModel>();

                if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                {
                    lista = prestamoResponse.Data
                        .Where(p => !string.IsNullOrWhiteSpace(p.TituloLibro))
                        .GroupBy(p => p.TituloLibro)
                        .Select(g => new LibroMasPrestadoViewModel
                        {
                            TituloLibro = g.Key,
                            CantidadPrestamos = g.Count()
                        })
                        .OrderByDescending(l => l.CantidadPrestamos)
                        .Take(20)
                        .ToList();
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al generar el reporte de libros más prestados.");
                return View("Error");
            }
        }

        // GET: Reporte/PrestamosPorUsuario
        public async Task<IActionResult> PrestamosPorUsuario()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();
                var lista = new List<PrestamoPorUsuarioViewModel>();

                if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                {
                    lista = prestamoResponse.Data
                        .GroupBy(p => p.NombreUsuario)
                        .Select(g => new PrestamoPorUsuarioViewModel
                        {
                            NombreUsuario = g.Key,
                            PrestamosActivos = g.Count(p => !string.Equals(p.EstadoPrestamo, "Devuelto", StringComparison.OrdinalIgnoreCase)),
                            PrestamosTotales = g.Count()
                        })
                        .OrderByDescending(x => x.PrestamosActivos)
                        .ToList();
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al generar el reporte de préstamos por usuario.");
                return View("Error");
            }
        }
    }
}