using SGI.Application.Dtos.Devolucion;
using SGI.WEB.Models;
using SGI.WEB.Models.Devolucion;
using System.Text.Json;

namespace SGI.WEB.Services
{
    public class DevolucionApiService : IDevolucionApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DevolucionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<DevolucionEditModel>>> GetDevoluciones()
        {
            var result = await _httpClient.GetAsync("Devolucion/GetDevoluciones");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<DevolucionEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de devoluciones.",
                    Data = null
                };
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<DevolucionEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<DevolucionEditModel>> GetDevolucionById(int id)
        {
            var result = await _httpClient.GetAsync($"Devolucion/GetDevolucionByID?devoid={id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<DevolucionEditModel>>(responseString, _jsonOptions);
        }

        public async Task<bool> CreateDevolucion(DevolucionCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Devolucion/CreateDevolucion", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ModifyDevolucion(DevolucionEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Devolucion/ModifyDevolucion", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DisabledDevolucion(int id)
        {
            var removeDto = new DevolucionRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Devolucion/DisabledDevolucion", removeDto);
            return result.IsSuccessStatusCode;
        }
    }
}
