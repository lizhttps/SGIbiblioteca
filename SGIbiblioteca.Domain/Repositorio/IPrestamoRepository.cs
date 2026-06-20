using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<List<Prestamo>> GetByUsuarioIdAsync(int usuarioId);
    }
}