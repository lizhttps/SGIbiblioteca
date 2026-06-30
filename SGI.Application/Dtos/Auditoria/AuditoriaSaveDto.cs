using SGI.Application.Base;

namespace SGI.Application.Dtos.Auditoria
{
    public class AuditoriaSaveDto
    {
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public string Accion { get; set; }
        public string RealizadoPor { get; set; }
        public string Detalle { get; set; }
    }
}