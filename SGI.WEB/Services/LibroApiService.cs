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
                return new ApiResponse<LibroEditModel>
                {
                    Success = false,
                    Message = "Error al obtener el libro.",
                    Data = default
                };
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<LibroEditModel>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> CreateLibro(LibroCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Libro/CreateLibro", model);
            var responseString = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
                return errorResponse ?? new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al crear el libro."
                };
            }

            return JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> ModifyLibro(LibroEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Libro/ModifyLibro", model);
            var responseString = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
                return errorResponse ?? new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al modificar el libro."
                };
            }

            return JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> DisabledLibro(int id)
        {
            var removeDto = new LibroRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Libro/DisabledLibro", removeDto);
            var responseString = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
                return errorResponse ?? new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al deshabilitar el libro."
                };
            }

            return JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
        }
    }
}
