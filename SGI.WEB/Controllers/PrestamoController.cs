using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGI.WEB.Models.Devolucion;
using SGI.WEB.Models.Notificacion;
using SGI.WEB.Models.Penalizacion;
using SGI.WEB.Models.Prestamo;
using SGI.WEB.Services.Devolucion;
using SGI.WEB.Services.Libro;
using SGI.WEB.Services.Notificacion;
using SGI.WEB.Services.Penalizacion;
using SGI.WEB.Services.Prestamo;
using SGI.WEB.Services.Usuario;
using SGIbiblioteca.Domain.Interfaces;
using System.Security.Claims;

namespace SGI.WEB.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly ILibroApiService _libroApiService;
        private readonly INotificacionApiService _notificacionApiService;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly ILoggerService _loggerService;

        public PrestamoController(
            IPrestamoApiService prestamoApiService,
            ILibroApiService libroApiService,
            INotificacionApiService notificacionApiService,
            IUsuarioApiService usuarioApiService,
            ILoggerService loggerService)
        {
            _prestamoApiService = prestamoApiService;
            _libroApiService = libroApiService;
            _notificacionApiService = notificacionApiService;
            _usuarioApiService = usuarioApiService;
            _loggerService = loggerService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();

                if (prestamoResponse != null && prestamoResponse.Success)
                {
                    if (!User.IsInRole("Bibliotecario"))
                    {
                        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                        var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                        prestamoResponse.Data = prestamoResponse.Data
                            .Where(p => p.UsuarioId == userId)
                            .ToList();
                    }
                    prestamoResponse.Data = prestamoResponse.Data
                        .Where(p => !string.Equals(p.EstadoPrestamo, "Devuelto", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                return View(prestamoResponse);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de préstamos.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var singleResponse = await _prestamoApiService.GetPrestamoById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    if (!User.IsInRole("Bibliotecario"))
                    {
                        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                        var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                        if (singleResponse.Data.UsuarioId != userId)
                        {
                            return Forbid();
                        }
                    }

                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener el préstamo con id {id}.");
                return View("Error");
            }
        }

        // GET: Prestamo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prestamo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoCreateModel prestamocreate)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                // Solo el Bibliotecario puede elegir para quién es el préstamo.
                if (!User.IsInRole("Bibliotecario"))
                {
                    prestamocreate.UsuarioId = userId;
                }

                prestamocreate.UsuarioMod = userId;
                prestamocreate.FechaMod = DateTime.Now;

                var response = await _prestamoApiService.CreatePrestamo(prestamocreate);

                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo crear el préstamo.");
                return View(prestamocreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el préstamo.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al crear el préstamo.");
                return View(prestamocreate);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var singleResponse = await _prestamoApiService.GetPrestamoById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el préstamo para editar.");
                return View("Error");
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                var response = await _prestamoApiService.ModifyPrestamo(model);

                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar el préstamo con id {model.Id}.");
                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo actualizar el préstamo.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar el préstamo.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al editar el préstamo.");
                return View(model);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var singleResponse = await _prestamoApiService.GetPrestamoById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el préstamo para eliminar.");
                return View("Error");
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _prestamoApiService.DisabledPrestamo(id);

                if (response == null || !response.Success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar el préstamo con id {id}.");
                    TempData["Error"] = response?.Message ?? "No se pudo eliminar el préstamo.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el préstamo.");
                TempData["Error"] = "Ocurrió un error inesperado al eliminar el préstamo.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Prestamo/ConfirmarSolicitud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarSolicitud(int libroId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                if (userId == 0)
                {
                    TempData["Error"] = "Debe iniciar sesión para solicitar un préstamo.";
                    return RedirectToAction("Index", "Libro");
                }

                var prestamoModel = new PrestamoCreateModel
                {
                    LibroId = libroId,
                    UsuarioId = userId,
                    FechaPrestamo = DateTime.Now,
                    FechaLimite = DateTime.Now.AddDays(7),
                    FechaDevolucionEsperada = DateTime.Now.AddDays(7),
                    Estado = "Pendiente",
                    UsuarioMod = userId,
                    FechaMod = DateTime.Now
                };

                var response = await _prestamoApiService.CreatePrestamo(prestamoModel);

                if (response != null && response.Success)
                {
                    var tituloLibro = "un libro";
                    try
                    {
                        var libroResponse = await _libroApiService.GetLibroById(libroId);
                        if (libroResponse?.Success == true && libroResponse.Data != null)
                        {
                            tituloLibro = libroResponse.Data.Titulo;
                        }
                    }
                    catch { }

                    var nombreUsuario = "Un usuario";
                    try
                    {
                        var usuariosResponse = await _usuarioApiService.GetUsuarios();
                        if (usuariosResponse?.Success == true && usuariosResponse.Data != null)
                        {
                            var solicitante = usuariosResponse.Data.FirstOrDefault(u => u.Id == userId);
                            if (solicitante != null)
                            {
                                nombreUsuario = $"{solicitante.Nombre} {solicitante.Apellido}";
                            }

                            var bibliotecarios = usuariosResponse.Data
                                .Where(u => !string.IsNullOrEmpty(u.Rol) &&
                                            u.Rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase))
                                .ToList();

                            _loggerService.LogWarning($"[DEBUG] Total usuarios: {usuariosResponse.Data.Count} | " +
                                $"Bibliotecarios encontrados: {bibliotecarios.Count} | " +
                                $"Roles vistos: {string.Join(", ", usuariosResponse.Data.Select(u => $"{u.Id}:'{u.Rol}'"))}");

                            foreach (var biblio in bibliotecarios)
                            {
                                var notificacion = new NotificacionCreateModel
                                {
                                    UsuarioId = biblio.Id,
                                    Mensaje = $"📬 Nueva solicitud de préstamo: \"{tituloLibro}\" " +
                                              $"solicitado por {nombreUsuario}. Revisa la sección de préstamos pendientes.",
                                    UsuarioMod = userId,
                                    FechaMod = DateTime.Now
                                };

                                await _notificacionApiService.CreateNotificacion(notificacion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _loggerService.LogError(ex, "Error al enviar notificaciones a bibliotecarios.");
                    }

                    TempData["Success"] = "📩 ¡Solicitud enviada! El bibliotecario la revisará y asignará la fecha de devolución.";
                    return RedirectToAction("Index", "Libro");
                }

                TempData["Error"] = response?.Message ?? "No se pudo enviar la solicitud de préstamo.";
                return RedirectToAction("Index", "Libro");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al procesar la solicitud de préstamo.");
                TempData["Error"] = "Ocurrió un error inesperado al procesar la solicitud.";
                return RedirectToAction("Index", "Libro");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Solicitar(int libroId)
        {
            try
            {
                var libroResponse = await _libroApiService.GetLibroById(libroId);

                if (libroResponse == null || !libroResponse.Success || libroResponse.Data == null)
                {
                    _loggerService.LogWarning($"No se encontró el libro con id {libroId} al solicitar préstamo.");
                    TempData["Error"] = "No se pudo encontrar el libro solicitado.";
                    return RedirectToAction("Index", "Libro");
                }

                return View(libroResponse.Data);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al cargar la solicitud de préstamo para el libro {libroId}.");
                TempData["Error"] = "Ocurrió un error inesperado al cargar la solicitud.";
                return RedirectToAction("Index", "Libro");
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id, DateTime fechaDevolucion)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                // Validar que la fecha sea futura
                if (fechaDevolucion.Date <= DateTime.Now.Date)
                {
                    TempData["Error"] = "La fecha de devolución debe ser posterior a hoy.";
                    return RedirectToAction(nameof(Index));
                }

                var response = await _prestamoApiService.AprobarPrestamo(id, userId, fechaDevolucion);

                if (response != null && response.Success)
                {
                    var prestamoResponse = await _prestamoApiService.GetPrestamoById(id);

                    if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                    {
                        var prestamo = prestamoResponse.Data;
                        var notificacion = new NotificacionCreateModel
                        {
                            UsuarioId = prestamo.UsuarioId,
                            Mensaje = $"¡Tu préstamo del libro \"{prestamo.TituloLibro}\" ha sido aprobado! " +
                                      $"Fecha límite de devolución: {fechaDevolucion:dd/MM/yyyy}. " +
                                      $"Tienes {(fechaDevolucion.Date - DateTime.Now.Date).Days} días para devolverlo.",
                            UsuarioMod = userId,
                            FechaMod = DateTime.Now
                        };

                        await _notificacionApiService.CreateNotificacion(notificacion);
                    }

                    TempData["Success"] = "Préstamo aprobado y notificación enviada al usuario.";
                }
                else
                {
                    TempData["Error"] = response?.Message ?? "No se pudo aprobar el préstamo.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al aprobar el préstamo con id {id}.");
                TempData["Error"] = "Ocurrió un error inesperado al aprobar el préstamo.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                // Obtener datos del préstamo ANTES de rechazar
                var prestamoResponse = await _prestamoApiService.GetPrestamoById(id);

                // Rechazar el préstamo
                var response = await _prestamoApiService.RechazarPrestamo(id, userId);

                if (response != null && response.Success)
                {
                    // Enviar notificación al estudiante
                    if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                    {
                        var prestamo = prestamoResponse.Data;

                        var notificacion = new NotificacionCreateModel
                        {
                            UsuarioId = prestamo.UsuarioId,
                            Mensaje = $"Tu solicitud de préstamo del libro \"{prestamo.TituloLibro}\" ha sido rechazada. " +
                                      $"Puedes contactar al bibliotecario para más información.",
                            UsuarioMod = userId,
                            FechaMod = DateTime.Now
                        };

                        await _notificacionApiService.CreateNotificacion(notificacion);
                    }

                    TempData["Success"] = "Préstamo rechazado y notificación enviada al usuario.";
                }
                else
                {
                    TempData["Error"] = response?.Message ?? "No se pudo rechazar el préstamo.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al rechazar el préstamo con id {id}.");
                TempData["Error"] = "Ocurrió un error inesperado al rechazar el préstamo.";
                return RedirectToAction(nameof(Index));
            }
        }



        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarDevuelto(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                // Traemos los datos del préstamo ANTES de marcarlo devuelto, para notificar
                var prestamoResponse = await _prestamoApiService.GetPrestamoById(id);

                var response = await _prestamoApiService.MarcarDevuelto(id, userId);

                if (response != null && response.Success)
                {
                    if (prestamoResponse?.Success == true && prestamoResponse.Data != null)
                    {
                        var prestamo = prestamoResponse.Data;
                        var notificacion = new NotificacionCreateModel
                        {
                            UsuarioId = prestamo.UsuarioId,
                            Mensaje = $"✅ Se registró la devolución del libro \"{prestamo.TituloLibro}\". ¡Gracias!",
                            UsuarioMod = userId,
                            FechaMod = DateTime.Now
                        };

                        await _notificacionApiService.CreateNotificacion(notificacion);
                    }

                    TempData["Success"] = "Préstamo marcado como devuelto correctamente.";
                }
                else
                {
                    TempData["Error"] = response?.Message ?? "No se pudo marcar el préstamo como devuelto.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al marcar como devuelto el préstamo con id {id}.");
                TempData["Error"] = "Ocurrió un error inesperado al marcar el préstamo como devuelto.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
