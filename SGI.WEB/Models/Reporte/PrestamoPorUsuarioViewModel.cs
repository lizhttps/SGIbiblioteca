namespace SGI.WEB.Models.Reporte
{
    public class PrestamoPorUsuarioViewModel
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public int PrestamosActivos { get; set; }
        public int PrestamosTotales { get; set; }
    }
}