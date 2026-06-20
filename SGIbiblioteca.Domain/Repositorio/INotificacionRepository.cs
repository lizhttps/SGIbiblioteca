using SGIbiblioteca.Domain.Entidades.Configuracion.Notificaciones;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<List<Notificacion>> GetByUsuarioIdAsync(int usuarioId);
    }
}