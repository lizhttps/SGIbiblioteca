using SGIbiblioteca.Domain.Entities.Penalizaciones;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<List<Penalizacion>> GetByUsuarioIdAsync(int usuarioId);
    }
}