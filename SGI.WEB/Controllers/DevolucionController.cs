using Microsoft.AspNetCore.Mvc;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Devolucion;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
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
    }
}
