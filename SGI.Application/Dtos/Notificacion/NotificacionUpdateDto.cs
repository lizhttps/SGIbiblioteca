

using SGI.Application.Base;

namespace SGI.Application.Dtos.Notificacion
{
    public class NotificacionUpdateDto : DtoBase
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
    }
}
