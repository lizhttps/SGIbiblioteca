

using SGI.Application.Base;

namespace SGI.Application.Dtos.Devolucion
{
    public class DevolucionSaveDto : DtoBase
    {
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public bool DevueltoATiempo { get; set; }
        public string Observaciones { get; set; }
    }
}
