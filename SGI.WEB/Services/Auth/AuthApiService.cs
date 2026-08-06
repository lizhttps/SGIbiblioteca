using SGI.Application.Dtos.Auth;
using SGI.Application.Dtos.Usuarios;
using SGI.WEB.Models;

namespace SGI.WEB.Services.Auth
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<UsuarioLoginResultDto>> LoginAsync(UsuarioLoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/Login", dto);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UsuarioLoginResultDto>>();

            return result ?? new ApiResponse<UsuarioLoginResultDto>
            {
                Success = false,
                Message = "Ocurrió un error al intentar iniciar sesión."
            };
        }

        public async Task<ApiResponse<object>> RegisterAsync(UsuarioRegisterDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/Register", dto);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            return result ?? new ApiResponse<object>
            {
                Success = false,
                Message = "Ocurrió un error al intentar registrar el usuario."
            };
        }
    }
}