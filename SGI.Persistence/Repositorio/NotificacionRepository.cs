using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entidades.Configuracion.Notificaciones;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public NotificacionRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                return await _context.Notificaciones.Where(n => n.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en NotificacionRepository al consultar notificaciones por usuarioId: {usuarioId}");
                return null;
            }
        }
    }
}
