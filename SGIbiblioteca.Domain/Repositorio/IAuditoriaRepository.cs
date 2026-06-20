using SGIbiblioteca.Domain.Entities.Auditorias;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IAuditoriaRepository : IBaseRepository<Auditoria>
    {
        Task<List<Auditoria>> GetByEntidadAsync(string entidad);
    }
}