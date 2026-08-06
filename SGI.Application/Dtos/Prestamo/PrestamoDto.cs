namespace SGI.Application.Dtos.Prestamo
{
    public class PrestamoDto
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public string EstadoPrestamo { get; set; } = string.Empty;
        public string TituloLibro { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
    }
}