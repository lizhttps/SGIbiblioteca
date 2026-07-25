
namespace SGI.WEB.Models.Penalizacion
{
    public class PenalizacionCreateModel
    {
        public int UsuarioId { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}
