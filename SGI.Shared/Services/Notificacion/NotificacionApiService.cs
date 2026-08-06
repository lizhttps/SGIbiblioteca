using SGI.WEB.Models;
using SGI.WEB.Models.Notificacion;
using System.Text.Json;
using System.Net.Http.Json;
using SGI.Shared.Models;


namespace SGI.WEB.Services.Notificacion
{
    public class NotificacionApiService : INotificacionApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public NotificacionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<NotificacionEditModel>>> GetNotificaciones()
        {
            var result = await _httpClient.GetAsync("Notificacion/GetNotificaciones");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<NotificacionEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de notificaciones.",
                    Data = null
                };
            }
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<NotificacionEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<List<NotificacionEditModel>>> GetNotificacionesByUsuario(int usuarioId)
        {
            // OJO: el parámetro en la API se llama "noti" aunque reciba el usuarioId
            var result = await _httpClient.GetAsync($"Notificacion/GetNotiByUsuario?noti={usuarioId}");
            if (!result.IsSuccessStatusCode)
            {
                return new ApiResponse<List<NotificacionEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener las notificaciones del usuario.",
                    Data = null
                };
            }
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<NotificacionEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<NotificacionEditModel>> GetNotificacionById(int id)
        {
            var result = await _httpClient.GetAsync($"Notificacion/GetNotiByID?id={id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<NotificacionEditModel>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> CreateNotificacion(NotificacionCreateModel model)
        {
            // Ruta con typo intencional para coincidir con la API: "CreateNoficacion"
            var result = await _httpClient.PostAsJsonAsync("Notificacion/CreateNoficacion", model);
            return await ReadApiResponse(result, "Error al crear la notificación.");
        }

        public async Task<ApiResponse<object>> ModifyNotificacion(NotificacionEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Notificacion/ModifyNotificacion", model);
            return await ReadApiResponse(result, "Error al modificar la notificación.");
        }

        public async Task<ApiResponse<object>> DisabledNotificacion(int id)
        {
            var removeDto = new { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Notificacion/DisabledNotificacion", removeDto);
            return await ReadApiResponse(result, "Error al eliminar la notificación.");
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
    }
}