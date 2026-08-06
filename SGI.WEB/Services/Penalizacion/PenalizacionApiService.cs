using SGI.WEB.Models;
using SGI.WEB.Models.Penalizacion;
using System.Text.Json;

namespace SGI.WEB.Services.Penalizacion
{
    public class PenalizacionApiService : IPenalizacionApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PenalizacionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<PenalizacionEditModel>>> GetPenalizaciones()
        {
            var result = await _httpClient.GetAsync("Penalizacion/GetPenalizacion");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<PenalizacionEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de penalizaciones.",
                    Data = null
                };
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<PenalizacionEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<PenalizacionEditModel>> GetPenalizacionById(int id)
        {
            var result = await _httpClient.GetAsync($"Penalizacion/GetPenalizacionByID?penaid={id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<PenalizacionEditModel>>(responseString, _jsonOptions);
        }

        public async Task<bool> CreatePenalizacion(PenalizacionCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Penalizacion/CreatePenalizacion", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ModifyPenalizacion(PenalizacionEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Penalizacion/ModifyPenalizacion", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DisabledPenalizacion(int id)
        {
            var result = await _httpClient.PostAsJsonAsync("Penalizacion/DisabledPenalizacion", new { id, estado = false });
            return result.IsSuccessStatusCode;
        }
    }
}
