using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entities.Penalizaciones;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public PenalizacionRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Penalizacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                return await _context.Penalizaciones.Where(p => p.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PenalizacionRepository al consultar penalizaciones por usuarioId: {usuarioId}");
                return null;
            }
        }
    }
}
