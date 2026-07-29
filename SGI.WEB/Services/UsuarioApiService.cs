using SGI.Application.Dtos.Usario;
using SGI.WEB.Models;
using SGI.WEB.Models.Usuario;
using System.Text.Json;

namespace SGI.WEB.Services
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

        public async Task<bool> CreateUsuario(UsuarioCreateModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Usuario/CreateUsuario", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ModifyUsuario(UsuarioEditModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("Usuario/ModifyUsuario", model);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DisabledUsuario(int id)
        {
            var dto = new UsuarioRemoveDto { Id = id, Estado = false };
            var result = await _httpClient.PostAsJsonAsync("Usuario/DisabledUsuario", dto);
            return result.IsSuccessStatusCode;
        }
    }
}