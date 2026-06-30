
using SGI.Application.Base;

namespace SGI.Application.Dtos.Prestamo
{
    public class PrestamoSaveDto : DtoBase
    {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}
