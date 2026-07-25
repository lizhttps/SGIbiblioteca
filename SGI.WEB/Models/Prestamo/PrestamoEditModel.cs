using System;

namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoEditModel
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime FechaMod { get; set; }
        public int UsuarioMod { get; set; }
    }
}
