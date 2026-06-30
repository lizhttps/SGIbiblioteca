

using SGI.Application.Base;

namespace SGI.Application.Dtos.Notificacion
{
    public class NotificacionSaveDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
    }
}
