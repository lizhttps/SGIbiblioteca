using SGI.Application.Dtos.Usuarios;
using SGI.Application.Dtos.Auth;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios;
using SGIbiblioteca.Domain.Interfaces;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoggerService _logger;

        public AuthService(IUsuarioRepository usuarioRepository, ILoggerService logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<OperationResult> LoginAsync(UsuarioLoginDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);
                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "El correo electrónico no se encuentra registrado.";
                    return result;
                }

                bool esClaveCorrecta = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
                if (!esClaveCorrecta)
                {
                    result.Success = false;
                    result.Message = "La contraseña es incorrecta.";
                    return result;
                }

                result.Success = true;
                result.Message = "Inicio de sesión exitoso.";
                result.Data = new UsuarioLoginResultDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    Rol = string.IsNullOrEmpty(usuario.Rol) ? "Estudiante" : usuario.Rol
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en el proceso de inicio de sesión.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> RegisterAsync(UsuarioRegisterDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                if (dto.Rol != "Estudiante" && dto.Rol != "Docente")
                {
                    result.Success = false;
                    result.Message = "El rol debe ser Estudiante o Docente.";
                    return result;
                }

                var existente = await _usuarioRepository.GetByCorreoAsync(dto.Correo);
                if (existente != null)
                {
                    result.Success = false;
                    result.Message = "Ese correo ya está registrado.";
                    return result;
                }

                result = await _usuarioRepository.SaveEntityAsync(new Usuario()
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Correo = dto.Correo,
                    Rol = dto.Rol,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Telefono = dto.Telefono,
                    Estado = true,
                    FechaCreacion = DateTime.Now,
                    CreadoPor = "RegistroAutonomo"
                });
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el Usuario";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}
