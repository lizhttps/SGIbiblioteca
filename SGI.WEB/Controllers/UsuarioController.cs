using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Usario;
using SGI.Application.Interfaces;
using SGI.WEB.Models;
using SGI.WEB.Models.Usuario;
using SGI.WEB.Services;

namespace SGI.WEB.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly ILoggerService _loggerService;

        public UsuarioController(IUsuarioApiService usuarioApiService, ILoggerService loggerService)
        {
            _usuarioApiService = usuarioApiService;
            _loggerService = loggerService;
        }

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            ApiResponse<List<UsuarioEditModel>> usuarioResponse = null;
            try
            {
                usuarioResponse = await _usuarioApiService.GetUsuarios();
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de usuarios.");
                return View("Error");
            }

            return View(usuarioResponse);
        }

        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ApiResponse<UsuarioEditModel> singleResponse = null;
            try
            {
                singleResponse = await _usuarioApiService.GetUsuarioById(id);

                if (singleResponse == null)
                {
                    _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}.");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener el usuario con id {id}.");
                return View("Error");
            }

            return View(singleResponse?.Data);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateModel usuariocreate)
        {
            try
            {
                var ok = await _usuarioApiService.CreateUsuario(usuariocreate);

                if (ok)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(usuariocreate);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el usuario.");
                return View(usuariocreate);
            }
        }

        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            UsuarioEditModel editModel = null;
            try
            {
                var singleResponse = await _usuarioApiService.GetUsuarioById(id);

                if (singleResponse != null && singleResponse.Success)
                {
                    editModel = singleResponse.Data;
                }
                else
                {
                    _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}.");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el usuario para editar.");
                return View("Error");
            }

            return View(editModel);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                var ok = await _usuarioApiService.ModifyUsuario(model);

                if (ok)
                {
                    return RedirectToAction(nameof(Index));
                }

                _loggerService.LogWarning($"No se pudo actualizar el usuario con id {model.Id}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al editar el usuario.");
                return View(model);
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ApiResponse<UsuarioEditModel> singleResponse = null;
            try
            {
                singleResponse = await _usuarioApiService.GetUsuarioById(id);

                if (singleResponse == null)
                {
                    _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}.");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el usuario para eliminar.");
                return View("Error");
            }

            return View(singleResponse?.Data);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var ok = await _usuarioApiService.DisabledUsuario(id);

                if (!ok)
                {
                    _loggerService.LogWarning($"No se pudo eliminar el usuario con id {id}.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el usuario.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}