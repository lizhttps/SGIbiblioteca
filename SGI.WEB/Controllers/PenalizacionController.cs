using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Notificacion;
using SGI.WEB.Models.Penalizacion;
using SGI.WEB.Services.Notificacion;
using SGI.WEB.Services.Penalizacion;
using SGI.WEB.Services.Prestamo;
using SGI.WEB.Services.Usuario;
using System.Security.Claims;

namespace SGI.WEB.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionApiService _penalizacionApiService;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly INotificacionApiService _notificacionApiService;
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly ILoggerService _loggerService;

        public PenalizacionController(
            IPenalizacionApiService penalizacionApiService,
            IUsuarioApiService usuarioApiService,
            INotificacionApiService notificacionApiService,
            IPrestamoApiService prestamoApiService,
            ILoggerService loggerService)
        {
            _penalizacionApiService = penalizacionApiService;
            _usuarioApiService = usuarioApiService;
            _notificacionApiService = notificacionApiService;
            _prestamoApiService = prestamoApiService;
            _loggerService = loggerService;
        }

        // GET: Penalizacion
        public async Task<IActionResult> Index()
        {
            try
            {
                var penalizacionResponse = await _penalizacionApiService.GetPenalizaciones();
                return View(penalizacionResponse);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de penalizaciones.");
                return View("Error");
            }
        }

        // GET: Penalizacion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var singleResponse = await _penalizacionApiService.GetPenalizacionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener la penalización con id {id}.");
                return View("Error");
            }
        }

        // GET: Penalizacion/Create
        public async Task<IActionResult> Create(int? usuarioId, int? prestamoId, string returnUrl)
        {
            var model = new PenalizacionCreateModel();

            if (usuarioId.HasValue && usuarioId.Value > 0)
            {
                model.UsuarioId = usuarioId.Value;

                try
                {
                    var usuariosResponse = await _usuarioApiService.GetUsuarios();
                    var usuario = usuariosResponse?.Data?.FirstOrDefault(u => u.Id == usuarioId.Value);
                    if (usuario != null)
                    {
                        ViewBag.NombreUsuario = $"{usuario.Nombre} {usuario.Apellido}";
                    }
                }
                catch (Exception ex)
                {
                    _loggerService.LogError(ex, "Error al obtener datos del usuario para la penalización.");
                }
            }

            ViewBag.PrestamoId = prestamoId;
            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        // POST: Penalizacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel createModel, int? prestamoId, string returnUrl)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                createModel.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                createModel.FechaMod = DateTime.Now;

                var success = await _penalizacionApiService.CreatePenalizacion(createModel);

                if (success)
                {
                    // 1. Notificación de la penalización al usuario
                    try
                    {
                        var notificacion = new NotificacionCreateModel
                        {
                            UsuarioId = createModel.UsuarioId,
                            Mensaje = $"⚠️ Se te ha registrado una penalización de ${createModel.Monto:0.00}. " +
                                      $"Motivo: {createModel.Motivo}",
                            UsuarioMod = createModel.UsuarioMod,
                            FechaMod = DateTime.Now
                        };

                        await _notificacionApiService.CreateNotificacion(notificacion);
                    }
                    catch (Exception ex)
                    {
                        _loggerService.LogError(ex, "Error al notificar al usuario sobre la penalización.");
                    }

                    // 2. Si la penalización viene ligada a un préstamo, márcalo como devuelto
                    if (prestamoId.HasValue && prestamoId.Value > 0)
                    {
                        try
                        {
                            await _prestamoApiService.MarcarDevuelto(prestamoId.Value, createModel.UsuarioMod);
                        }
                        catch (Exception ex)
                        {
                            _loggerService.LogError(ex, $"Error al marcar como devuelto el préstamo {prestamoId} tras penalización.");
                        }
                    }

                    // 3. Redirección
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.PrestamoId = prestamoId;
                ViewBag.ReturnUrl = returnUrl;
                return View(createModel);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear la penalización.");
                ViewBag.PrestamoId = prestamoId;
                ViewBag.ReturnUrl = returnUrl;
                return View(createModel);
            }
        }

        // GET: Penalizacion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var singleResponse = await _penalizacionApiService.GetPenalizacionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la penalización para editar.");
                return View("Error");
            }
        }

        // POST: Penalizacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                var success = await _penalizacionApiService.ModifyPenalizacion(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar la penalización con id {model.Id}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar la penalización.");
                return View(model);
            }
        }

        // GET: Penalizacion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var singleResponse = await _penalizacionApiService.GetPenalizacionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la penalización para eliminar.");
                return View("Error");
            }
        }

        // POST: Penalizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _penalizacionApiService.DisabledPenalizacion(id);
                if (!success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar la penalización con id {id}.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar la penalización.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
