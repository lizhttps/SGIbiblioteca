using SGI.Application.Dtos.Auth;
using SGI.Application.Dtos.Usuarios;
using SGI.WEB.Models;

namespace SGI.WEB.Services.Auth
{
    public interface IAuthApiService
    {
        Task<ApiResponse<UsuarioLoginResultDto>> LoginAsync(UsuarioLoginDto dto);
        Task<ApiResponse<object>> RegisterAsync(UsuarioRegisterDto dto);
    }
}
