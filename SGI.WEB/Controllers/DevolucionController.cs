using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Devolucion;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Devolucion;
using System.Text.Json;

namespace SGI.WEB.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly ILogger<DevolucionController> _logger;
        private readonly string _apiBaseUrl = "https://localhost:7289/api/";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DevolucionController(ILoggerService loggerService, ILogger<DevolucionController> logger)
        {
            _loggerService = loggerService;
            _logger = logger;
        }

        // GET: Devolucion
        public async Task<IActionResult> Index()
        {
            DevolucionResponse devolucionResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync("Devolucion/GetDevoluciones");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        devolucionResponse = JsonSerializer.Deserialize<DevolucionResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        devolucionResponse = new DevolucionResponse
                        {
                            Success = false,
                            Message = "Error al obtener la lista de devoluciones.",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de devoluciones.");
                return View("Error");
            }

            return View(devolucionResponse);
        }

        // GET: Devolucion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            DevolucionSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Devolucion/GetDevolucionByID?devoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<DevolucionSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _logger.LogWarning("No se pudo obtener la devolución con id {Id}. Status: {Status}", id, result.StatusCode);
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la devolución con id {Id}.", id);
                return View("Error");
            }

            return View(singleResponse?.Data);
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.PostAsJsonAsync("Devolucion/CreateDevolucion", devolucioncreate);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(devolucioncreate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la devolución.");
                return View(devolucioncreate);
            }
        }

        // GET: Devolucion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            DevolucionEditModel editModel = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Devolucion/GetDevolucionByID?devoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = JsonSerializer.Deserialize<DevolucionSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            editModel = singleResponse.Data;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No se pudo obtener la devolución con id {Id}. Status: {Status}", id, result.StatusCode);
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la devolución para editar.");
                return View("Error");
            }

            return View(editModel);
        }

        // POST: Devolucion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DevolucionEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var response = await client.PostAsJsonAsync("Devolucion/ModifyDevolucion", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _logger.LogWarning("No se pudo actualizar la devolución con id {Id}. Status: {Status}", model.Id, response.StatusCode);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la devolución.");
                return View(model);
            }
        }

        // GET: Devolucion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            DevolucionSingleResponse singleResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Devolucion/GetDevolucionByID?devoid={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        singleResponse = JsonSerializer.Deserialize<DevolucionSingleResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _logger.LogWarning("No se pudo obtener la devolución con id {Id}. Status: {Status}", id, result.StatusCode);
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la devolución para eliminar.");
                return View("Error");
            }

            return View(singleResponse?.Data);
        }

        // POST: Devolucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var removeDto = new DevolucionRemoveDto { Id = id, Estado = false };

                    var response = await client.PostAsJsonAsync("Devolucion/DisabledDevolucion", removeDto);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _logger.LogWarning("No se pudo eliminar la devolución con id {Id}. Status: {Status}", id, response.StatusCode);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la devolución.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
