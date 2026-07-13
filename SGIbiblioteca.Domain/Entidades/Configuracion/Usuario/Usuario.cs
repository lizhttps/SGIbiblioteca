using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios
{
    public class Usuario : AuditEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string? Telefono { get; set; }
    }
}