using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Prestamo;
using System.Text.Json;

namespace SGI.WEB.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly string _apiBaseUrl = "https://localhost:7289/api/";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PrestamoController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        // GET: Prestamo
        public async Task<IActionResult> Index()
        {
            PrestamoResponse prestamoResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync("Prestamo/GetPrestamo");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        prestamoResponse = JsonSerializer.Deserialize<PrestamoResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        prestamoResponse = new PrestamoResponse
                        {
                            Success = false,
                            Message = "Error al obtener la lista de préstamos.",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de préstamos.");
                return View("Error");
            }

            return View(prestamoResponse);
        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            PrestamoSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Prestamo/GetPrestamoID?prestamoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<PrestamoSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener el préstamo con id {id}.");
                return View("Error");
            }

            return View(singleResponse?.Data);
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.PostAsJsonAsync("Prestamo/CreatePrestamo", prestamocreate);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(prestamocreate);
                }
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
            PrestamoEditModel editModel = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Prestamo/GetPrestamoID?prestamoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = JsonSerializer.Deserialize<PrestamoSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            editModel = singleResponse.Data;
                        }
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el préstamo para editar.");
                return View("Error");
            }

            return View(editModel);
        }

        // POST: Prestamo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var response = await client.PostAsJsonAsync("Prestamo/ModifyPrestamo", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo actualizar el préstamo con id {model.Id}. Status: {response.StatusCode}");
                    return View(model);
                }
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
            PrestamoSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Prestamo/GetPrestamoID?prestamoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<PrestamoSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el préstamo con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el préstamo para eliminar.");
                return View("Error");
            }

            return View(singleResponse?.Data);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var removeDto = new PrestamoRemoveDto { Id = id, Estado = false };

                    var response = await client.PostAsJsonAsync("Prestamo/DisabledPrestamo", removeDto);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo eliminar el préstamo con id {id}. Status: {response.StatusCode}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el préstamo.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
