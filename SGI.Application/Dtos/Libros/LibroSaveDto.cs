
using SGI.Application.Base;

namespace SGI.Application.Dtos.Libros
{
    public class LibroSaveDto : DtoBase
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Categoria { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadDisponible { get; set; }
        public string Estado { get; set; }
    }
}
