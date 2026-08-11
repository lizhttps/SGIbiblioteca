using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Notificacion;
using System.Security.Claims;
using SGI.WEB.Services.Notificacion;

namespace SGI.WEB.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly INotificacionApiService _notificacionApiService;
        private readonly ILoggerService _loggerService;

        public NotificacionController(INotificacionApiService notificacionApiService, ILoggerService loggerService)
        {
            _notificacionApiService = notificacionApiService;
            _loggerService = loggerService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (!User.IsInRole("Bibliotecario"))
                {
                    return RedirectToAction(nameof(MisNotificaciones));
                }

                var response = await _notificacionApiService.GetNotificaciones();
                return View(response);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de notificaciones.");
                return View("Error");
            }
        }


        // Notificaciones del usuario logueado
        public async Task<IActionResult> MisNotificaciones()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                var response = await _notificacionApiService.GetNotificacionesByUsuario(userId);
                return View(response);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener las notificaciones del usuario.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var singleResponse = await _notificacionApiService.GetNotificacionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la notificación con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener la notificación con id {id}.");
                return View("Error");
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificacionCreateModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                var response = await _notificacionApiService.CreateNotificacion(model);

                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(MisNotificaciones));
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo crear la notificación.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear la notificación.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al crear la notificación.");
                return View(model);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var singleResponse = await _notificacionApiService.GetNotificacionById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener la notificación con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la notificación para editar.");
                return View("Error");
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NotificacionEditModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                var response = await _notificacionApiService.ModifyNotificacion(model);

                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(MisNotificaciones));
                }

                _loggerService.LogWarning($"No se pudo actualizar la notificación con id {model.Id}.");
                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo actualizar la notificación.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar la notificación.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al editar la notificación.");
                return View(model);
            }
        }

        // Marcar como leída (acción rápida, útil para notificaciones)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarLeida(int id)
        {
            try
            {
                var singleResponse = await _notificacionApiService.GetNotificacionById(id);
                if (singleResponse == null || !singleResponse.Success || singleResponse.Data == null)
                {
                    TempData["Error"] = "No se encontró la notificación.";
                    return RedirectToAction(nameof(MisNotificaciones));
                }

                var model = singleResponse.Data;
                model.Leido = true;

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                await _notificacionApiService.ModifyNotificacion(model);

                return RedirectToAction(nameof(MisNotificaciones));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al marcar como leída la notificación {id}.");
                TempData["Error"] = "Ocurrió un error al marcar la notificación como leída.";
                return RedirectToAction(nameof(MisNotificaciones));
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _notificacionApiService.DisabledNotificacion(id);

                if (response == null || !response.Success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar la notificación con id {id}.");
                    TempData["Error"] = response?.Message ?? "No se pudo eliminar la notificación.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar la notificación.");
                TempData["Error"] = "Ocurrió un error inesperado al eliminar la notificación.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}