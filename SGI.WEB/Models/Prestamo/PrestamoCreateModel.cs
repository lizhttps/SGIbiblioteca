
namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoCreateModel
    {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaLimite { get; set; } = DateTime.Now.AddDays(7);
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}
