using SGI.Application.Base;
using SGI.Application.Dtos.Penalizacion;
using SGIbiblioteca.Domain.Base;


namespace SGI.Application.Interfaces
{
    public interface IPenalizacionService : IBaseService<PenalizacionSaveDto, PenalizacionUpdateDto, PenalizacionRemoveDto>
    {
        Task<OperationResult> GetPenalizacionesByUsuarioId(int usuarioId);
        Task<OperationResult> GetPenalizacionesVencidasByUsuarioId(int usuarioId);
    }
}
