using Microsoft.AspNetCore.Mvc;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Prestamo;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly ILoggerService _loggerService;

        public PrestamoController(IPrestamoApiService prestamoApiService, ILoggerService loggerService)
        {
            _prestamoApiService = prestamoApiService;
            _loggerService = loggerService;
        }

        // GET: Prestamo
        public async Task<IActionResult> Index()
        {
            try
            {
                var prestamoResponse = await _prestamoApiService.GetPrestamos();
                return View(prestamoResponse);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de préstamos.");
                return View("Error");
            }
        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int id)
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
                var success = await _prestamoApiService.CreatePrestamo(prestamocreate);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(prestamocreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el préstamo.");
                return View(prestamocreate);
            }
        }

        // GET: Prestamo/Edit/5
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

        // POST: Prestamo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                var success = await _prestamoApiService.ModifyPrestamo(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar el préstamo con id {model.Id}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar el préstamo.");
                return View(model);
            }
        }

        // GET: Prestamo/Delete/5
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

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _prestamoApiService.DisabledPrestamo(id);
                if (!success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar el préstamo con id {id}.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el préstamo.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
