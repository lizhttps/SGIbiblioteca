using SGI.Application.Dtos.Usario;
using SGIbiblioteca.Domain.Base;
using SGI.Application.Base;

namespace SGI.Application.Interfaces
{
    public interface IUsuarioService : IBaseService<UsuarioSaveDto, UsuarioUpdateDto, UsuarioRemoveDto>
    {
        Task<OperationResult> GetCorreo (string correo);
    }
}
