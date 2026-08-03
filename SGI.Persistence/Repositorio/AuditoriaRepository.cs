using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entities.Auditorias;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Persistence.Repositorios
{
    // Implementación del repositorio de auditoría. obtiene los datos de auditoría de la base de datos.
    public class AuditoriaRepository : BaseRepository<Auditoria>, IAuditoriaRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public AuditoriaRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Auditoria>> GetByEntidadAsync(string entidad)
        {
            try
            {
                return await _context.Auditorias.Where(a => a.Entidad == entidad).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en AuditoriaRepository al consultar por entidad: {entidad}");
                return null;
            }
        }
    }
}
