using SGI.WEB.Models;
using SGI.WEB.Models.Usuario;

namespace SGI.WEB.Services
{
    public interface IUsuarioApiService
    {
        Task<ApiResponse<List<UsuarioEditModel>>> GetUsuarios();
        Task<ApiResponse<UsuarioEditModel>> GetUsuarioById(int id);
        Task<bool> CreateUsuario(UsuarioCreateModel model);
        Task<bool> ModifyUsuario(UsuarioEditModel model);
        Task<bool> DisabledUsuario(int id);
    }
}