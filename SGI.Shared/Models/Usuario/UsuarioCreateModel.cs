namespace SGI.WEB.Models.Usuario
{
    public class UsuarioCreateModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaMod { get; set; } = DateTime.Now;
        public int UsuarioMod { get; set; }
    }
}
