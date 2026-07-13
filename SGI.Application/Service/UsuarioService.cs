using SGI.Application.Dtos.Usario;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios;
using SGIbiblioteca.Domain.Repositorio;


namespace SGI.Application.Service
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly ILoggerService _logger;

        public UsuarioService(IUsuarioRepository UsuarioRepository, ILoggerService logger)
        {
            _UsuarioRepository = UsuarioRepository;
            _logger = logger;
        }
        public async Task<OperationResult> GetCorreo(string correo)
        {
            OperationResult result = new OperationResult();

            try
            {
                var usuariocorreo = await _UsuarioRepository.GetByCorreoAsync(correo);

                if (usuariocorreo == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado con ese correo.";
                    return result;
                }

                var usuarioResult = new UsuarioUpdateDto()
                {
                    Id = usuariocorreo.Id,
                    Nombre = usuariocorreo.Nombre,
                    Apellido = usuariocorreo.Apellido,
                    Correo = usuariocorreo.Correo,
                    Telefono = usuariocorreo.Telefono,
                    FechaMod = usuariocorreo.FechaCreacion,
                    UsuarioMod = int.TryParse(usuariocorreo.CreadoPor, out int user) ? user : 0
                };

                result.Data = usuarioResult;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Error obteniendo";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }


        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _UsuarioRepository.GetAllAsync())
                    .Select(Usuario => new UsuarioUpdateDto()
                    {
                        Id = Usuario.Id,
                        Nombre = Usuario.Nombre,
                        Apellido = Usuario.Apellido,
                        Correo = Usuario.Correo,
                        Telefono = Usuario.Telefono,
                        FechaMod = Usuario.FechaCreacion,
                        UsuarioMod = int.TryParse(Usuario.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los usuarios";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuarioid = await _UsuarioRepository.GetEntityByIdAsync(id);

                if (usuarioid == null)
                {
                    result.Success = false;
                    result.Message = "Registro no encontrado.";
                    return result;
                }

                var usuarioresult = new UsuarioUpdateDto()
                {
                    Id = usuarioid.Id,
                    Nombre = usuarioid.Nombre,
                    Apellido = usuarioid.Apellido,
                    Correo = usuarioid.Correo,
                    Telefono = usuarioid.Telefono,
                    FechaMod = usuarioid.FechaCreacion,
                    UsuarioMod = int.TryParse(usuarioid.CreadoPor, out int user) ? user : 0
                };
                result.Data = usuarioresult;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Remove(UsuarioRemoveDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var UsuarioToDelete = await _UsuarioRepository.GetEntityByIdAsync(dto.Id);

                if (UsuarioToDelete == null) // si el usario no existe, devolvemos un error.
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }
                UsuarioToDelete.Estado = dto.Estado;

                // usuario no tiene una entidad de estado,p


                await _UsuarioRepository.UpdateEntityAsync(UsuarioToDelete);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al intentar eliminar el Usuario.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }


        public async Task<OperationResult> Save(UsuarioSaveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _UsuarioRepository.SaveEntityAsync(new Usuario()
                {


                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Correo = dto.Correo,
                    Telefono = dto.Telefono,
                    FechaCreacion = dto.FechaMod,
                    Estado = true,
                    CreadoPor = dto.UsuarioMod.ToString()


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


        public async Task<OperationResult> Update(UsuarioUpdateDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var UsuarioToUpdate = await _UsuarioRepository.GetEntityByIdAsync(dto.Id);

                if (UsuarioToUpdate == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }

                UsuarioToUpdate.Nombre = dto.Nombre;
                UsuarioToUpdate.Apellido = dto.Apellido;
                UsuarioToUpdate.Correo = dto.Correo;
                UsuarioToUpdate.Telefono = dto.Telefono;
                UsuarioToUpdate.FechaModificacion = dto.FechaMod;
                UsuarioToUpdate.ModificadoPor = dto.UsuarioMod.ToString();

                await _UsuarioRepository.UpdateEntityAsync(UsuarioToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el Usuario";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}
