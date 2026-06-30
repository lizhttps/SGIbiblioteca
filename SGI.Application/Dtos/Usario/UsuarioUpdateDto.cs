

using SGI.Application.Base;

namespace SGI.Application.Dtos.Usario
{
    public class UsuarioUpdateDto : DtoBase
    { 
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }
}
