using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public PrestamoRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Prestamo>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                return await _context.Prestamos.Where(p => p.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PrestamoRepository al consultar préstamos por usuarioId: {usuarioId}");
                return null;
            }
        }
    }
}
