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
        private readonly IDevolucionApiService _devolucionApiService;
        private readonly ILibroApiService _libroApiService;
        private readonly ILoggerService _loggerService;

        public ReporteController(
            IPrestamoApiService prestamoApiService,
            IDevolucionApiService devolucionApiService,
            ILibroApiService libroApiService,
            ILoggerService loggerService)
        {
            _prestamoApiService = prestamoApiService;
            _devolucionApiService = devolucionApiService;
            _libroApiService = libroApiService;
            _loggerService = loggerService;
        }

        // GET: Reporte
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reporte/PrestamosVencidos
        public async Task<IActionResult> PrestamosVencidos()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();
                var lista = new List<PrestamoVencidoViewModel>();

                if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                {
                    var hoy = DateTime.Now.Date;

                    lista = prestamoResponse.Data
                        .Where(p => !string.Equals(p.EstadoPrestamo, "Devuelto", StringComparison.OrdinalIgnoreCase)
                                 && p.FechaLimite.Date < hoy) // AJUSTAR: confirma el nombre del campo de fecha límite
                        .Select(p => new PrestamoVencidoViewModel
                        {
                            PrestamoId = p.Id,
                            TituloLibro = p.TituloLibro,
                            NombreUsuario = p.NombreUsuario, // AJUSTAR si el campo se llama distinto
                            FechaLimite = p.FechaLimite,
                            DiasVencido = (hoy - p.FechaLimite.Date).Days
                        })
                        .OrderByDescending(p => p.DiasVencido)
                        .ToList();
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al generar el reporte de préstamos vencidos.");
                return View("Error");
            }
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

        // GET: Reporte/DevolucionesTardias
        public async Task<IActionResult> DevolucionesTardias()
        {
            try
            {
                var devolucionResponse = await _devolucionApiService.GetDevoluciones();
                var prestamoResponse = await _prestamoApiService.GetPrestamos();
                var lista = new List<DevolucionTardiaViewModel>();

                if (devolucionResponse?.Success == true && devolucionResponse.Data != null
                    && prestamoResponse?.Success == true && prestamoResponse.Data != null)
                {
                    foreach (var d in devolucionResponse.Data)
                    {
                        var prestamo = prestamoResponse.Data.FirstOrDefault(p => p.Id == d.PrestamoId);
                        if (prestamo == null) continue;

                        // AJUSTAR: confirma los nombres de campos de fecha en Devolucion y Prestamo
                        if (d.FechaDevolucion.Date > prestamo.FechaLimite.Date)
                        {
                            lista.Add(new DevolucionTardiaViewModel
                            {
                                PrestamoId = prestamo.Id,
                                TituloLibro = prestamo.TituloLibro,
                                NombreUsuario = prestamo.NombreUsuario,
                                FechaLimite = prestamo.FechaLimite,
                                FechaDevolucion = d.FechaDevolucion,
                                DiasTardanza = (d.FechaDevolucion.Date - prestamo.FechaLimite.Date).Days
                            });
                        }
                    }

                    lista = lista.OrderByDescending(x => x.DiasTardanza).ToList();
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al generar el reporte de devoluciones tardías.");
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