using SGI.Application.Dtos.Prestamo;
using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;
using System.Text.Json;

namespace SGI.WEB.Services.Prestamo
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

        public async Task<ApiResponse<object>> CreatePrestamo(PrestamoCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Prestamo/CreatePrestamo", model);
            return await ReadApiResponse(result, "Error al crear el préstamo.");
        }

        public async Task<ApiResponse<object>> ModifyPrestamo(PrestamoEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Prestamo/ModifyPrestamo", model);
            return await ReadApiResponse(result, "Error al modificar el préstamo.");
        }

        public async Task<ApiResponse<object>> DisabledPrestamo(int id)
        {
            var removeDto = new PrestamoRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Prestamo/DisabledPrestamo", removeDto);
            return await ReadApiResponse(result, "Error al eliminar el préstamo.");
        }

        private static async Task<ApiResponse<object>> ReadApiResponse(HttpResponseMessage result, string fallbackMessage)
        {
            var responseString = await result.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseString))
            {
                return new ApiResponse<object>
                {
                    Success = result.IsSuccessStatusCode,
                    Message = result.IsSuccessStatusCode ? null : fallbackMessage,
                    Data = null
                };
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonOptions);
                return parsed ?? new ApiResponse<object>
                {
                    Success = result.IsSuccessStatusCode,
                    Message = fallbackMessage,
                    Data = null
                };
            }
            catch (JsonException)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = fallbackMessage,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamosByUsuario(int usuarioId)
        {
            var result = await _httpClient.GetAsync($"Prestamo/GetPrestamosByUsuario?usuarioId={usuarioId}");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<PrestamoEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener los préstamos del usuario.",
                    Data = null
                };
            }
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<PrestamoEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> AprobarPrestamo(int id, int usuarioMod, DateTime fechaDevolucion)
        {
            var decisionDto = new PrestamoDecisionDto
            {
                Id = id,
                UsuarioMod = usuarioMod,
                FechaDevolucion = fechaDevolucion  
            };
            var result = await _httpClient.PostAsJsonAsync("Prestamo/AprobarPrestamo", decisionDto);
            return await ReadApiResponse(result, "Error al aprobar el préstamo.");
        }


        public async Task<ApiResponse<object>> RechazarPrestamo(int id, int usuarioMod)
        {
            var decisionDto = new PrestamoDecisionDto { Id = id, UsuarioMod = usuarioMod };
            var result = await _httpClient.PostAsJsonAsync("Prestamo/RechazarPrestamo", decisionDto);
            return await ReadApiResponse(result, "Error al rechazar el préstamo.");
        }
    }
}