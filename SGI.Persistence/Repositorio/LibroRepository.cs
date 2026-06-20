using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class LibroRepository : BaseRepository<Libro>, ILibroRepository
    {
        private readonly SigebiContext _context;

        public LibroRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Libro> GetByISBNAsync(string isbn)
        {
            return await _context.Libros.FirstOrDefaultAsync(l => l.ISBN == isbn);
        }
    }
}