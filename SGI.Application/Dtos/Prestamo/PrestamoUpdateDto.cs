

using SGI.Application.Base;

namespace SGI.Application.Dtos.Prestamo
{
    public class PrestamoUpdateDto : DtoBase
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}
