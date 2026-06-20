using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entities.Auditorias;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class AuditoriaRepository : BaseRepository<Auditoria>, IAuditoriaRepository
    {
        private readonly SigebiContext _context;

        public AuditoriaRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Auditoria>> GetByEntidadAsync(string entidad)
        {
            return await _context.Auditorias.Where(a => a.Entidad == entidad).ToListAsync();
        }
    }
}