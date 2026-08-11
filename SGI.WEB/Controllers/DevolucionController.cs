using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Devolucion;
using SGI.WEB.Models.Notificacion; // NUEVO
using SGI.WEB.Models.Prestamo;
using System.Security.Claims;
using SGI.WEB.Services.Devolucion;
using SGI.WEB.Services.Prestamo;
using SGI.WEB.Services.Notificacion; // NUEVO

namespace SGI.WEB.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class DevolucionController : Controller
    {
        private readonly IDevolucionApiService _devolucionApiService;
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly INotificacionApiService _notificacionApiService; // NUEVO
        private readonly ILoggerService _loggerService;

        public DevolucionController(
            IDevolucionApiService devolucionApiService,
            IPrestamoApiService prestamoApiService,
            INotificacionApiService notificacionApiService, // NUEVO
            ILoggerService loggerService)
        {
            _devolucionApiService = devolucionApiService;
            _prestamoApiService = prestamoApiService;
            _notificacionApiService = notificacionApiService; // NUEVO
            _loggerService = loggerService;
        }

        // GET: Devolucion
        public async Task<IActionResult> Index()
        {
            try
            {
                var devolucionResponse = await _devolucionApiService.GetDevoluciones();
                return View(devolucionResponse);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de devoluciones.");
                return View("Error");
            }
        }

        // GET: Devolucion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var singleResponse = await _devolucionApiService.GetDevolucionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la devolución con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener la devolución con id {id}.");
                return View("Error");
            }
        }

        // GET: Devolucion/Create
        public async Task<IActionResult> Create(int? prestamoId, string returnUrl)
        {
            var model = new DevolucionCreateModel();

            if (prestamoId.HasValue)
            {
                model.PrestamoId = prestamoId.Value;

                // Validación para evitar abrir la vista si ya existe devolución
                if (await YaFueDevuelto(prestamoId.Value))
                {
                    TempData["Error"] = "Este préstamo ya fue marcado como devuelto anteriormente.";
                    return Redirect(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? returnUrl
                        : Url.Action(nameof(Index)));
                }

                // Carga el nombre de usuario para mostrarlo en la vista si está disponible
                try
                {
                    var prestamoResponse = await _prestamoApiService.GetPrestamoById(prestamoId.Value);
                    var prestamo = prestamoResponse?.Data;
                    if (prestamo != null && !string.IsNullOrWhiteSpace(prestamo.NombreUsuario))
                    {
                        ViewBag.NombreUsuario = prestamo.NombreUsuario;
                    }
                }
                catch (Exception ex)
                {
                    _loggerService.LogError(ex, "Error al obtener datos del préstamo para la devolución.");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: Devolucion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DevolucionCreateModel devolucioncreate, string returnUrl)
        {
            try
            {
                // Validación para evitar procesar devoluciones duplicadas
                if (await YaFueDevuelto(devolucioncreate.PrestamoId))
                {
                    TempData["Error"] = "Este préstamo ya fue marcado como devuelto anteriormente.";
                    return Redirect(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? returnUrl
                        : Url.Action(nameof(Index)));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                devolucioncreate.UsuarioMod = userId;
                devolucioncreate.FechaMod = DateTime.Now;

                var success = await _devolucionApiService.CreateDevolucion(devolucioncreate);
                if (success)
                {
                    // Obtenemos el préstamo una sola vez para actualizar estado y notificar
                    PrestamoEditModel? prestamo = null;
                    try
                    {
                        var prestamoResponse = await _prestamoApiService.GetPrestamoById(devolucioncreate.PrestamoId);
                        prestamo = prestamoResponse?.Data;
                    }
                    catch (Exception ex)
                    {
                        _loggerService.LogError(ex, $"Error al obtener el préstamo {devolucioncreate.PrestamoId} para notificación.");
                    }

                    // Actualizamos el estado del préstamo a "Devuelto"
                    try
                    {
                        await _prestamoApiService.MarcarDevuelto(devolucioncreate.PrestamoId, userId);
                    }
                    catch (Exception ex)
                    {
                        _loggerService.LogError(ex, $"Error al marcar como devuelto el préstamo {devolucioncreate.PrestamoId} tras registrar devolución.");
                    }

                    // Notificamos al usuario solicitante sobre la devolución
                    if (prestamo != null)
                    {
                        try
                        {
                            var notificacion = new NotificacionCreateModel
                            {
                                UsuarioId = prestamo.UsuarioId,
                                Mensaje = "✅ Tu devolución fue registrada correctamente.",
                                UsuarioMod = userId,
                                FechaMod = DateTime.Now
                            };

                            await _notificacionApiService.CreateNotificacion(notificacion);
                        }
                        catch (Exception ex)
                        {
                            _loggerService.LogError(ex, "Error al notificar al usuario sobre la devolución.");
                        }
                    }

                    TempData["Success"] = "Devolución registrada correctamente.";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(devolucioncreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al registrar la devolución.");
                ViewBag.ReturnUrl = returnUrl;
                return View(devolucioncreate);
            }
        }

        // GET: Devolucion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var singleResponse = await _devolucionApiService.GetDevolucionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la devolución con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la devolución para editar.");
                return View("Error");
            }
        }

        // POST: Devolucion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DevolucionEditModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                var success = await _devolucionApiService.ModifyDevolucion(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar la devolución con id {model.Id}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar la devolución.");
                return View(model);
            }
        }

        // GET: Devolucion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var singleResponse = await _devolucionApiService.GetDevolucionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la devolución con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la devolución para eliminar.");
                return View("Error");
            }
        }

        // POST: Devolucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _devolucionApiService.DisabledDevolucion(id);
                if (!success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar la devolución con id {id}.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar la devolución.");
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper: revisa si ya existe una devolución para ese préstamo
        private async Task<bool> YaFueDevuelto(int prestamoId)
        {
            try
            {
                var devolucionesResponse = await _devolucionApiService.GetDevoluciones();
                return devolucionesResponse?.Success == true
                    && devolucionesResponse.Data != null
                    && devolucionesResponse.Data.Any(d => d.PrestamoId == prestamoId);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al verificar devoluciones previas del préstamo {prestamoId}.");
                // Si falla la verificación, mejor bloquear por seguridad que dejar pasar un duplicado
                return true;
            }
        }
    }
}
