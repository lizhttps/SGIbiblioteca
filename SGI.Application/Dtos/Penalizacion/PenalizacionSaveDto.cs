

using SGI.Application.Base;

namespace SGI.Application.Dtos.Penalizacion
{
    public class PenalizacionSaveDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
    }
}
