using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entidades.Configuracion.Notificaciones;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly SigebiContext _context;

        public NotificacionRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notificacion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Notificaciones.Where(n => n.UsuarioId == usuarioId).ToListAsync();
        }
    }
}