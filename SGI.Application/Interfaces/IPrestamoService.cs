using SGI.Application.Base;
using SGI.Application.Dtos.Prestamo;
using SGIbiblioteca.Domain.Base;

namespace SGI.Application.Interfaces
{
    public interface IPrestamoService : IBaseService<PrestamoSaveDto, PrestamoUpdateDto, PrestamoRemoveDto>
    {
        Task<OperationResult> GetPrestamosByPrestamoId(int usuarioid);

        Task<OperationResult> GetVencidosByUsuarioId(int usuarioId);
    }
}