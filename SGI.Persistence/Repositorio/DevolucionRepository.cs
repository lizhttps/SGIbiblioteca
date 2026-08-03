using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entidades.Configuracion.Devoluciones;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    public class DevolucionRepository : BaseRepository<Devolucion>, IDevolucionRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public DevolucionRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        // Implementación del método para obtener una devolución por el ID del préstamo.
        public async Task<Devolucion> GetByPrestamoIdAsync(int prestamoId)
        {
            try
            {
                return await _context.Devoluciones.FirstOrDefaultAsync(d => d.PrestamoId == prestamoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DevolucionRepository al consultar la devolución por prestamoId: {prestamoId}");
                return null;
            }
        }
    }
}
