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
                // Asignación de datos auditables y estado por defecto para el nuevo libro
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                librocreate.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
                librocreate.FechaMod = DateTime.Now;
                librocreate.Estado = "true"; // Estado activo por defecto al crear

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
                // Asignar el ID del usuario logueado que realiza la modificación
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                model.UsuarioMod = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
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
