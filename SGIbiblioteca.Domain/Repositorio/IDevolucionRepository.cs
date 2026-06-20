using SGIbiblioteca.Domain.Entidades.Configuracion.Devoluciones;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IDevolucionRepository : IBaseRepository<Devolucion>
    {
        Task<Devolucion> GetByPrestamoIdAsync(int prestamoId);
    }
}