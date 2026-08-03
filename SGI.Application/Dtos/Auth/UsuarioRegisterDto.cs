namespace SGI.Application.Dtos.Auth
{
    public class UsuarioRegisterDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
    }
}