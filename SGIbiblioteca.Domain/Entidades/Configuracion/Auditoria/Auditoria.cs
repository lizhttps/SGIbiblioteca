using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entities.Auditorias
{
    public class Auditoria : AuditEntity
    {
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public int EntidadId { get; set; }
        public string Detalle { get; set; }
        public string RealizadoPor { get; set; }
        public DateTime FechaAccion { get; set; }
    }
}