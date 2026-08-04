using SGI.WEB.Models;
using SGI.WEB.Models.Notificacion;

namespace SGI.WEB.Services.Notificacion
{
    public interface INotificacionApiService
    {
        Task<ApiResponse<List<NotificacionEditModel>>> GetNotificaciones();
        Task<ApiResponse<List<NotificacionEditModel>>> GetNotificacionesByUsuario(int usuarioId);
        Task<ApiResponse<NotificacionEditModel>> GetNotificacionById(int id);
        Task<ApiResponse<object>> CreateNotificacion(NotificacionCreateModel model);
        Task<ApiResponse<object>> ModifyNotificacion(NotificacionEditModel model);
        Task<ApiResponse<object>> DisabledNotificacion(int id);
    }
}