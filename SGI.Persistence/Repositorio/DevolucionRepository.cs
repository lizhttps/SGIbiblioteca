using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entidades.Configuracion.Devoluciones;
using SGIbiblioteca.Domain.Repositorio;
using SGI.Persistence.context;
using SGI.Persistence.Base;

namespace SGI.Persistence.Repositorios
{
    public class DevolucionRepository : BaseRepository<Devolucion>, IDevolucionRepository
    {
        private readonly SigebiContext _context;

        public DevolucionRepository(SigebiContext context) : base(context)
        {
            _context = context;
        }

        // Implementación del método para obtener una devolución por el ID del préstamo.
        public async Task<Devolucion> GetByPrestamoIdAsync(int prestamoId)
        {
            return await _context.Devoluciones.FirstOrDefaultAsync(d => d.PrestamoId == prestamoId);
        }
    }
}