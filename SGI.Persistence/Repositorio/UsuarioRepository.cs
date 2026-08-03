using Microsoft.EntityFrameworkCore;
using SGI.Persistence.Base;
using SGI.Persistence.context;
using SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;


namespace SGI.Persistence.Repositorios
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly SigebiContext _context;
        private readonly ILoggerService _logger;

        public UsuarioRepository(SigebiContext context, ILoggerService logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Usuario> GetByCorreoAsync(string correo)
        {
            try
            {
                return await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correo && u.Estado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en UsuarioRepository al consultar el correo: {correo}");
                return null;
            }
        }
    }
}
