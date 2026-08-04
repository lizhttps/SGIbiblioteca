using SGI.Application.Dtos.Usario;
using SGI.WEB.Models;
using SGI.WEB.Models.Usuario;
using System.Text.Json;

namespace SGI.WEB.Services.Usuario
{
    public class UsuarioApiService : IUsuarioApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public UsuarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<UsuarioEditModel>>> GetUsuarios()
        {
            var result = await _httpClient.GetAsync("Usuario/GetUsuario");
            if (!result.IsSuccessStatusCode)
                return new ApiResponse<List<UsuarioEditModel>>
                {
                    Success = false,
                    Message = "Error al obtener la lista de usuarios."
                };
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<UsuarioEditModel>>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<UsuarioEditModel>> GetUsuarioById(int id)
        {
            var result = await _httpClient.GetAsync($"Usuario/GetUsuarioByID?usuarioId={id}");
            if (!result.IsSuccessStatusCode) return null;
            var responseString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<UsuarioEditModel>>(responseString, _jsonOptions);
        }

        public async Task<ApiResponse<object>> CreateUsuario(UsuarioCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Usuario/CreateUsuario", model);
            return await ReadApiResponse(result, "Error al crear el usuario.");
        }

        public async Task<ApiResponse<object>> ModifyUsuario(UsuarioEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Usuario/ModifyUsuario", model);
            return await ReadApiResponse(result, "Error al modificar el usuario.");
        }

        public async Task<ApiResponse<object>> DisabledUsuario(int id)
        {
            var dto = new UsuarioRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Usuario/DisabledUsuario", dto);
            return await ReadApiResponse(result, "Error al eliminar el usuario.");
        }

        // Helper: lee el cuerpo de la respuesta aunque el status no sea 2xx,
        // porque el OperationResult con el mensaje de negocio viaja en el body.
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