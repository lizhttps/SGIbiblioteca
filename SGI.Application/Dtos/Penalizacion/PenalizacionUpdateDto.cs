

using SGI.Application.Base;

namespace SGI.Application.Dtos.Penalizacion
{
    public class PenalizacionUpdateDto : DtoBase
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public bool Pagada { get; set; }
    }
}
