using Microsoft.AspNetCore.Mvc;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Penalizacion;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionApiService _penalizacionApiService;
        private readonly ILoggerService _loggerService;

        public PenalizacionController(IPenalizacionApiService penalizacionApiService, ILoggerService loggerService)
        {
            _penalizacionApiService = penalizacionApiService;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Penalizacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel createModel)
        {
            try
            {
                var success = await _penalizacionApiService.CreatePenalizacion(createModel);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(createModel);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear la penalización.");
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
