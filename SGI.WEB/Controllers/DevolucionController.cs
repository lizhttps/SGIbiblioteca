using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Devolucion;
using System.Security.Claims;
using SGI.WEB.Services.Devolucion;

namespace SGI.WEB.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class DevolucionController : Controller
    {
        private readonly IDevolucionApiService _devolucionApiService;
        private readonly ILoggerService _loggerService;

        public DevolucionController(IDevolucionApiService devolucionApiService, ILoggerService loggerService)
        {
            _devolucionApiService = devolucionApiService;
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
                devolucioncreate.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                devolucioncreate.FechaMod = DateTime.Now;

                var success = await _devolucionApiService.CreateDevolucion(devolucioncreate);
                if (success)
                {
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
