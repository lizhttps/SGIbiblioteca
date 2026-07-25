namespace SGI.WEB.Models.Libro
{
    public class LibroCreateModel
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Categoria { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadDisponible { get; set; }
        public string Estado { get; set; }
        public int UsuarioMod { get; set; }
        public DateTime FechaMod { get; set; }

    }
}
