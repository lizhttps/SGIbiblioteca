using SGI.Application.Dtos.Notificacion;
using SGIbiblioteca.Domain.Base;
using SGI.Application.Base;

namespace SGI.Application.Interfaces
{
    public interface INotificacionService : IBaseService<NotificacionSaveDto, NotificacionUpdateDto, NotificacionRemoveDto>
    {
        Task<OperationResult> GetNotiUsuario (int usuarioId);
    }
}
