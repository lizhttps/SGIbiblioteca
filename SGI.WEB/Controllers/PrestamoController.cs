using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;
using SGI.WEB.Services.Prestamo;
using SGIbiblioteca.Domain.Interfaces;
using System.Security.Claims;

namespace SGI.WEB.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly ILoggerService _loggerService;

        public PrestamoController(IPrestamoApiService prestamoApiService, ILoggerService loggerService)
        {
            _prestamoApiService = prestamoApiService;
            _loggerService = loggerService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();

                if (prestamoResponse != null && prestamoResponse.Success && !User.IsInRole("Bibliotecario"))
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                    prestamoResponse.Data = prestamoResponse.Data
                        .Where(p => p.UsuarioId == userId)
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
    }
}