using SGI.Application.Base;
using SGI.Application.Dtos.Devolucion;
using SGIbiblioteca.Domain.Base;


namespace SGI.Application.Interfaces
{
    public interface IDevolucionService : IBaseService<DevolucionSaveDto, DevolucionUpdateDto, DevolucionRemoveDto>
    {
        Task<OperationResult> GetDevolucionesByUsuarioId(int usuarioId);

        Task<OperationResult> GetByPrestamoId(int prestamoId);
    }
}
