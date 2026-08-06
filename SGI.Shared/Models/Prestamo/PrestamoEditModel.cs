namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoEditModel
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public string Estado { get; set; } = string.Empty; 
        public int UsuarioMod { get; set; }
        public DateTime FechaMod { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string EstadoPrestamo { get; set; } = string.Empty;
    }
}
