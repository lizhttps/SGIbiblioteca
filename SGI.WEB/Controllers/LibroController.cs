using Microsoft.AspNetCore.Mvc;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Libro;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibroApiService _libroApiService;
        private readonly ILoggerService _loggerService;

        public LibroController(ILibroApiService libroApiService, ILoggerService loggerService)
        {
            _libroApiService = libroApiService;
            _loggerService = loggerService;
        }

        // GET: LibroController
        public async Task<IActionResult> Index()
        {
            try
            {
                var libroResponse = await _libroApiService.GetLibros();
                return View(libroResponse);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de libros.");
                return View("Error");
            }
        }

        // GET: LibroController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var singleResponse = await _libroApiService.GetLibroById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener el libro con id {id}.");
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LibroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroCreateModel librocreate)
        {
            try
            {
                var success = await _libroApiService.CreateLibro(librocreate);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(librocreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el libro.");
                return View(librocreate);
            }
        }

        // GET: Libro/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var singleResponse = await _libroApiService.GetLibroById(id);
                if (singleResponse != null && singleResponse.Success && singleResponse.Data != null)
                {
                    return View(singleResponse.Data);
                }

                _loggerService.LogWarning($"No se pudo obtener el libro con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el libro para editar.");
                return View("Error");
            }
        }

        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibroEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                var success = await _libroApiService.ModifyLibro(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar el libro con id {model.Id}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar el libro.");
                return View(model);
            }
        }

        // GET: LibroController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var singleResponse = await _libroApiService.GetLibroById(id);
                if (singleResponse != null)
                {
                    return View(singleResponse);
                }

                _loggerService.LogWarning($"No se pudo obtener el libro con id {id}.");
                return View("Error");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el libro para eliminar.");
                return View("Error");
            }
        }

        // POST: LibroController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _libroApiService.DisabledLibro(id);
                if (!success)
                {
                    _loggerService.LogWarning($"No se pudo eliminar el libro con id {id}.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el libro.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
