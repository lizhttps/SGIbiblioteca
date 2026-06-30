

namespace SGI.Application.Base
{
    public class DtoBase
    {
        public int UsuarioMod { get; set; }
        // transportar el ID del usuario que está usando el sistema en ese momento
        public DateTime FechaMod { get; set; }
        // transportar la fecha en que se está haciendo la modificación
    }
}
