using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos
{
    public class Prestamo : AuditEntity
    {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}