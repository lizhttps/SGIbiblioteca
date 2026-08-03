using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Devolucion;
using SGI.WEB.Services;
using System.Security.Claims;

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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Devolucion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DevolucionCreateModel devolucioncreate)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                devolucioncreate.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                devolucioncreate.FechaMod = DateTime.Now;

                var success = await _devolucionApiService.CreateDevolucion(devolucioncreate);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(devolucioncreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al registrar la devolución.");
                return View(devolucioncreate);
            }
        }

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
    }
}