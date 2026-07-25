using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Usario;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Usuario;
using System.Text.Json;

namespace SGI.WEB.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly string _apiBaseUrl = "https://localhost:7289/api/";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public UsuarioController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            UsuarioResponse usuarioResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync("Usuario/GetUsuario");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        usuarioResponse = JsonSerializer.Deserialize<UsuarioResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        usuarioResponse = new UsuarioResponse
                        {
                            Success = false,
                            Message = "Error al obtener la lista de usuarios.",
                            Data = null
                        };
                    }
                }
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
            UsuarioSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Usuario/GetUsuarioByID?usuarioid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<UsuarioSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.PostAsJsonAsync("Usuario/CreateUsuario", usuariocreate);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(usuariocreate);
                }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Usuario/GetUsuarioByID?usuarioid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = JsonSerializer.Deserialize<UsuarioSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            editModel = singleResponse.Data;
                        }
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);

                    var response = await client.PostAsJsonAsync("Usuario/ModifyUsuario", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo actualizar el usuario con id {model.Id}. Status: {response.StatusCode}");
                    return View(model);
                }
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
            UsuarioSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Usuario/GetUsuarioByID?usuarioid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<UsuarioSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el usuario con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var removeDto = new UsuarioRemoveDto { Id = id, Estado = false };

                    var response = await client.PostAsJsonAsync("Usuario/DisabledUsuario", removeDto);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo eliminar el usuario con id {id}. Status: {response.StatusCode}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el usuario.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
