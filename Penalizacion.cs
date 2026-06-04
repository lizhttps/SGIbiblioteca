using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entities.Penalizaciones
{
    public class Penalizacion : AuditEntity
    {
        public int UsuarioId { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public bool Pagada { get; set; }
    }
}