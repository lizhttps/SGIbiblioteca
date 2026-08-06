using SGI.Shared.Models;
using SGI.WEB.Models.Usuario;

namespace SGI.WEB.Services.Usuario
{
    public interface IUsuarioApiService
    {
        Task<ApiResponse<List<UsuarioEditModel>>> GetUsuarios();
        Task<ApiResponse<UsuarioEditModel>> GetUsuarioById(int id);
        Task<ApiResponse<object>> CreateUsuario(UsuarioCreateModel model);
        Task<ApiResponse<object>> ModifyUsuario(UsuarioEditModel model);
        Task<ApiResponse<object>> DisabledUsuario(int id);
    }
}