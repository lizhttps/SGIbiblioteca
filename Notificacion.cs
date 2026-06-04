using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entities.Notificacion
{
    public class Notificacion : AuditEntity
    {
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leido { get; set; }
    }
}