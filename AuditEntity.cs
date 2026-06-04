using System;

namespace SGIbiblioteca.Domain.Base
{
    public class AuditEntity
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public bool Estado { get; set; }
    }
}