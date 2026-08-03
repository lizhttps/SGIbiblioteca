using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public LibroRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        // Implementación del método para obtener un libro por su ISBN.
        public async Task<Libro> GetByISBNAsync(string isbn)
        {
            try
            {
                return await _context.Libros.FirstOrDefaultAsync(l => l.ISBN == isbn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en LibroRepository al consultar el ISBN: {isbn}");
                return null;
            }
        }
    }
}
