using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface ILibroRepository : IBaseRepository<Libro>
    {
        
        // METODOS FUTUROS
        Task<Libro> GetByISBNAsync(string isbn);
    }
}