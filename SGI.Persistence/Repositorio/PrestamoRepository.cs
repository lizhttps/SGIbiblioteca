using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly SigebiContext _context;

        public PrestamoRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Prestamo>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Prestamos.Where(p => p.UsuarioId == usuarioId).ToListAsync();
        }
    }
}