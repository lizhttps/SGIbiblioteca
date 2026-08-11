
using SGI.Application.Dtos.Usuarios;

namespace SGI.Shared.Session
{
    public static class UserSession
    {
        public static int Id { get; private set; }
        public static string Nombre { get; private set; } = string.Empty;
        public static string Apellido { get; private set; } = string.Empty;
        public static string Correo { get; private set; } = string.Empty;
        public static string Rol { get; private set; } = string.Empty;

        public static bool IsAuthenticated => Id != 0;
        public static bool EsBibliotecario => Rol == "Bibliotecario";

        public static void IniciarSesion(UsuarioLoginResultDto dto)
        {
            Id = dto.Id;
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            Correo = dto.Correo;
            Rol = dto.Rol;
        }

        public static void CerrarSesion()
        {
            Id = 0;
            Nombre = Apellido = Correo = Rol = string.Empty;
        }
    }
}