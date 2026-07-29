using SGI.Application.Dtos.Prestamo;
using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;
using System.Text.Json;

namespace SGI.WEB.Services
{
    public class PrestamoApiService : IPrestamoApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PrestamoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamos()
        {
            var result = await _httpClient.GetAsync("Prestamo/GetPrestamo");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<PrestamoEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de préstamos.",
                    Data = null
                };
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<PrestamoEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<PrestamoEditModel>> GetPrestamoById(int id)
        {
            var result = await _httpClient.GetAsync($"Prestamo/GetPrestamoID?prestamoid={id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<PrestamoEditModel>>(responseString, _jsonOptions);
        }

        public async Task<bool> CreatePrestamo(PrestamoCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Prestamo/CreatePrestamo", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ModifyPrestamo(PrestamoEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Prestamo/ModifyPrestamo", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DisabledPrestamo(int id)
        {
            var removeDto = new PrestamoRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Prestamo/DisabledPrestamo", removeDto);
            return result.IsSuccessStatusCode;
        }
    }
}
