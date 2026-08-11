using System;

namespace SGI.WEB.Models.Devolucion
{
    public class DevolucionCreateModel
    {
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucion { get; set; } = DateTime.Now;
        public bool DevueltoATiempo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}
