namespace SGI.WEB.Models.Reporte
{
    public class PrestamoVencidoViewModel
    {
        public int PrestamoId { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaLimite { get; set; }
        public int DiasVencido { get; set; }
    }
}