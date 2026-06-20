using SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario> GetByCorreoAsync(string correo);
    }
}