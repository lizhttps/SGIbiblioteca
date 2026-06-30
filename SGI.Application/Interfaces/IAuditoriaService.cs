using SGI.Application.Dtos.Auditoria;
using SGIbiblioteca.Domain.Base;

namespace SGI.Application.Interfaces
{
    public interface IAuditoriaService
    {
        Task<OperationResult> Save(AuditoriaSaveDto dto);
        Task<OperationResult> GetData();
        Task<OperationResult> GetDataById(int id);
        Task<OperationResult> GetByEntidad(string entidad);
    }
}
