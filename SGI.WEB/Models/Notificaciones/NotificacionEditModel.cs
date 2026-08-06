namespace SGI.WEB.Models.Notificacion
{
    public class NotificacionEditModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}