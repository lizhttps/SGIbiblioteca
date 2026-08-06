namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoCreateModel
    {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaLimite { get; set; } = DateTime.Now.AddDays(7); 
        public DateTime FechaDevolucionEsperada { get; set; } = DateTime.Now.AddDays(7);
        public string Estado { get; set; } = "Pendiente";

        public int UsuarioMod { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
    }
}
