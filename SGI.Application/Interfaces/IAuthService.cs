using SGI.Application.Dtos.Auth;
using SGIbiblioteca.Domain.Base;

namespace SGI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(UsuarioLoginDto dto);
        Task<OperationResult> RegisterAsync(UsuarioRegisterDto dto);
    }
}
