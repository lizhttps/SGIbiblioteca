namespace SGI.WEB.Models.Notificacion
{
    public class NotificacionCreateModel
    {
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}