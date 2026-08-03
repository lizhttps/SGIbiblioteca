using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.WEB.Models.Libro;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
    [Authorize] // Exige estar logueado (cualquier rol)
    public class LibroController : Controller
    {
        private readonly ILibroApiService _libroApiService;
        private readonly ILoggerService _loggerService;

        public LibroController(ILibroApiService libroApiService, ILoggerService loggerService)
        {
            _libroApiService = libroApiService;
            _loggerService = loggerService;
        }

        // PÚBLICO PARA CUALQUIER ROL (Estudiante, Docente, Bibliotecario)
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

        // PÚBLICO PARA CUALQUIER ROL
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

        // --- DE AQUÍ EN ADELANTE SOLO BIBLIOTECARIO ---

        [Authorize(Roles = "Bibliotecario")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroCreateModel librocreate)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                librocreate.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                librocreate.FechaMod = DateTime.Now;
                librocreate.Estado = "true";

                var response = await _libroApiService.CreateLibro(librocreate);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo crear el libro.");
                return View(librocreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el libro.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al crear el libro.");
                return View(librocreate);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
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

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibroEditModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                model.FechaMod = DateTime.Now;

                var response = await _libroApiService.ModifyLibro(model);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "No se pudo actualizar el libro.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar el libro.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al editar el libro.");
                return View(model);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
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

        [Authorize(Roles = "Bibliotecario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _libroApiService.DisabledLibro(id);
                if (response == null || !response.Success)
                {
                    TempData["Error"] = response?.Message ?? "No se pudo eliminar el libro.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el libro.");
                TempData["Error"] = "Ocurrió un error inesperado al eliminar el libro.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
