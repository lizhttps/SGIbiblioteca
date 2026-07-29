using SGI.Application.Dtos.Libros;
using SGI.WEB.Models;
using SGI.WEB.Models.Libro;
using System.Text.Json;

namespace SGI.WEB.Services
{
    public class LibroApiService : ILibroApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public LibroApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<LibroEditModel>>> GetLibros()
        {
            var result = await _httpClient.GetAsync("Libro/GetLibros");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<LibroEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de libros.",
                    Data = null
                };
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<LibroEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<LibroEditModel>> GetLibroById(int id)
        {
            var result = await _httpClient.GetAsync($"Libro/GetLibroByID?id={id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<LibroEditModel>>(responseString, _jsonOptions);
        }

        public async Task<bool> CreateLibro(LibroCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Libro/CreateLibro", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ModifyLibro(LibroEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Libro/ModifyLibro", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DisabledLibro(int id)
        {
            var removeDto = new LibroRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Libro/DisabledLibro", removeDto);
            return result.IsSuccessStatusCode;
        }
    }
}
