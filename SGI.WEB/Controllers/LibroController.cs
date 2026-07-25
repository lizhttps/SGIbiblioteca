using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Libros;
using SGI.Application.Interfaces;
using SGI.WEB.Models.Libro;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;

namespace SGI.WEB.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly string _apiBaseUrl = "https://localhost:7289/api/";

        private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public LibroController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        // GET: LibroController
        public async Task<IActionResult> Index()
        {
            LibroResponse libroResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync("Libro/GetLibros");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        libroResponse = System.Text.Json.JsonSerializer.Deserialize<LibroResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        libroResponse = new LibroResponse
                        {
                            Success = false,
                            Message = "Error al obtener la lista de libros.",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de libros.");
                return View("Error");
            }

            return View(libroResponse);
        }

        // GET: LibroController/Details/5
        // GET: LibroController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            LibroEditModel libro = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Libro/GetLibroByID?id={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = System.Text.Json.JsonSerializer.Deserialize<LibroSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            libro = singleResponse.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener el libro con id {id}.");
                return View("Error");
            }

            if (libro == null) return RedirectToAction(nameof(Index));
            return View(libro);
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);

                    var result = await client.PostAsJsonAsync("Libro/CreateLibro", librocreate);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(librocreate);
                }
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
            LibroEditModel editModel = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Libro/GetLibroByID?id={id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var singleResponse = System.Text.Json.JsonSerializer.Deserialize<LibroSingleResponse>(responseString, _jsonOptions);

                        if (singleResponse != null && singleResponse.Success)
                        {
                            editModel = singleResponse.Data; 
                        }
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el libro con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el libro para editar.");
                return View("Error");
            }

            if (editModel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(editModel); 
        }


        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibroEditModel model)
        {
            try
            {
                model.FechaMod = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);

                    var response = await client.PostAsJsonAsync("Libro/ModifyLibro", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo actualizar el libro con id {model.Id}. Status: {response.StatusCode}");
                    return View(model);
                }
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
            LibroResponse libroResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var result = await client.GetAsync($"Libro/GetLibroByID/{id}");

                    if (result.IsSuccessStatusCode)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        libroResponse = System.Text.Json.JsonSerializer.Deserialize<LibroResponse>(responseString, _jsonOptions);
                    }
                    else
                    {
                        _loggerService.LogWarning($"No se pudo obtener el libro con id {id}. Status: {result.StatusCode}");
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener el libro para eliminar.");
                return View("Error");
            }

            return View(libroResponse);
        }

        // POST: LibroController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var removeDto = new LibroRemoveDto { Id = id, Estado = false };

                    var response = await client.PostAsJsonAsync("Libro/DisabledLibro", removeDto);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _loggerService.LogWarning($"No se pudo eliminar el libro con id {id}. Status: {response.StatusCode}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al eliminar el libro.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
