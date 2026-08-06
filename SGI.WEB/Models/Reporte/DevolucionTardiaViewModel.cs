namespace SGI.WEB.Models.Reporte
{
    public class DevolucionTardiaViewModel
    {
        public int PrestamoId { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaLimite { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int DiasTardanza { get; set; }
    }
}