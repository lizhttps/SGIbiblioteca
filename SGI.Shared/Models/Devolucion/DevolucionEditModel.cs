using System;

namespace SGI.WEB.Models.Devolucion
{
    public class DevolucionEditModel
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public bool DevueltoATiempo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}
