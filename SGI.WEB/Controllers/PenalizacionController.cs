using Microsoft.AspNetCore.Mvc;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Penalizacion;
using System.Text.Json;

namespace SGI.WEB.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly string _apiBaseUrl = "https://localhost:7289/api/";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PenalizacionController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        // GET: Penalizacion
        public async Task<IActionResult> Index()
        {
            PenalizacionResponse penalizacionResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync("Penalizacion/GetPenalizacion");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        penalizacionResponse = JsonSerializer.Deserialize<PenalizacionResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        penalizacionResponse = new PenalizacionResponse
                        {
                            Success = false,
                            Message = "Error al obtener la lista de penalizaciones.",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de penalizaciones.");
                return View("Error");
            }

            return View(penalizacionResponse);
        }

        // GET: Penalizacion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            PenalizacionSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Penalizacion/GetPenalizacionByID?penaid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<PenalizacionSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener la penalización con id {id}.");
                return View("Error");
            }

            return View(singleResponse?.Data);
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.PostAsJsonAsync("Penalizacion/CreatePenalizacion", createModel);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(createModel);
                }
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
            PenalizacionEditModel editModel = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Penalizacion/GetPenalizacionByID?penaid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = JsonSerializer.Deserialize<PenalizacionSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            editModel = singleResponse.Data;
                        }
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la penalización para editar.");
                return View("Error");
            }

            return View(editModel);
        }

        // POST: Penalizacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);

                    var response = await client.PostAsJsonAsync("Penalizacion/ModifyPenalizacion", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo actualizar la penalización con id {model.Id}. Status: {response.StatusCode}");
                    return View(model);
                }
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
            PenalizacionSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Penalizacion/GetPenalizacionByID?penaid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<PenalizacionSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener la penalización con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la penalización para eliminar.");
                return View("Error");
            }

            return View(singleResponse?.Data);
        }

        // POST: Penalizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var response = await client.PostAsJsonAsync("Penalizacion/DisabledPenalizacion", new { id = id, estado = false });

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo eliminar la penalización con id {id}. Status: {response.StatusCode}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar la penalización.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
