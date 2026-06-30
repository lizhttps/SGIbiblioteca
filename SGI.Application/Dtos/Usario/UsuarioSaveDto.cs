

using SGI.Application.Base;

namespace SGI.Application.Dtos.Usario
{
    public class UsuarioSaveDto : DtoBase
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }
}
