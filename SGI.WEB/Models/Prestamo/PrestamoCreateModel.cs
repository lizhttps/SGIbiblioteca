namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoCreateModel
    {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime? FechaLimite { get; set; }             
        public DateTime? FechaDevolucionEsperada { get; set; }  
        public string Estado { get; set; } = "Pendiente";

        public int UsuarioMod { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
    }
}
