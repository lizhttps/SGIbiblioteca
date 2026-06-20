using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entities.Penalizaciones;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly SigebiContext _context;

        public PenalizacionRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Penalizacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Penalizaciones.Where(p => p.UsuarioId == usuarioId).ToListAsync();
        }
    }
}