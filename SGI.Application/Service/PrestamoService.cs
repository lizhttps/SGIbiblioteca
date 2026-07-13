using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;
using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPenalizacionRepository _penalizacionRepository;
        private readonly ILoggerService _logger;

        public PrestamoService(
            IPrestamoRepository prestamoRepository,
            ILibroRepository libroRepository,
            IUsuarioRepository usuarioRepository,
            IPenalizacionRepository penalizacionRepository,
            ILoggerService logger)
        {
            _prestamoRepository = prestamoRepository;
            _libroRepository = libroRepository;
            _usuarioRepository = usuarioRepository;
            _penalizacionRepository = penalizacionRepository;
            _logger = logger;
        }

        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _prestamoRepository.GetAllAsync())
                    .Select(p => new PrestamoUpdateDto()
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        LibroId = p.LibroId,
                        FechaLimite = p.FechaLimite,
                        FechaMod = p.FechaCreacion,
                        UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener los prestamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "prestamo no encontrado.";
                    return result;
                }
                result.Data = new PrestamoUpdateDto()
                {
                    Id = prestamo.Id,
                    UsuarioId = prestamo.UsuarioId,
                    LibroId = prestamo.LibroId,
                    FechaLimite = prestamo.FechaLimite,
                    FechaMod = prestamo.FechaCreacion,
                    UsuarioMod = int.TryParse(prestamo.CreadoPor, out int user) ? user : 0
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener el prestamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Save(PrestamoSaveDto dto)
        {
            OperationResult result = new OperationResult();
            Libro? libro = null;
            bool libroDescontado = false;

            try
            {
                // validaciones
                var usuario = await _usuarioRepository.GetEntityByIdAsync(dto.UsuarioId);
                if (usuario == null || usuario.Estado == false)
                {
                    result.Success = false;
                    result.Message = "El usuario no es válido o está inactivo";
                    return result;
                }

                var penalizaciones = await _penalizacionRepository.GetByUsuarioIdAsync(dto.UsuarioId);
                if (penalizaciones != null && penalizaciones.Any(p => p.Pagada == false)) // Verifica si hay penalizaciones pendientes de pago
                {
                    result.Success = false;
                    result.Message = "El usuario tiene penalizaciones pendientes de pago";
                    return result;
                }

                libro = await _libroRepository.GetEntityByIdAsync(dto.LibroId);
                if (libro == null)
                {
                    result.Success = false;
                    result.Message = "El libro solicitado no existe";
                    return result;
                }

                if (libro.CantidadDisponible <= 0)
                {
                    result.Success = false;
                    result.Message = "no hay copias disponibles de este libro";
                    return result;
                }

                // Descontamos la disponibilidad del libro
                libro.CantidadDisponible = libro.CantidadDisponible - 1;
                await _libroRepository.UpdateEntityAsync(libro);
                libroDescontado = true;

                result = await _prestamoRepository.SaveEntityAsync(new Prestamo()
                {
                    LibroId = dto.LibroId,
                    UsuarioId = dto.UsuarioId,
                    FechaLimite = dto.FechaLimite,
                    FechaCreacion = dto.FechaMod,
                    CreadoPor = dto.UsuarioMod.ToString(),
                    Estado = true
                });

                // Si el préstamo no se pudo registrar, revertimos el descuento del libro
                // para no dejar una copia "fantasma" descontada sin préstamo asociado
                if (!result.Success && libroDescontado)
                {
                    libro.CantidadDisponible += 1;
                    await _libroRepository.UpdateEntityAsync(libro);
                }
            }
            catch (Exception ex)
            {
                // Si la excepción ocurrió después de descontar el libro, también revertimos aquí
                if (libroDescontado && libro != null)
                {
                    libro.CantidadDisponible += 1;
                    await _libroRepository.UpdateEntityAsync(libro);
                }

                result.Success = false;
                result.Message = "Error al registrar el prestamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }


        public async Task<OperationResult> Update(PrestamoUpdateDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(dto.Id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "prestamo no encontrado.";
                    return result;
                }

                prestamo.LibroId = dto.LibroId;
                prestamo.UsuarioId = dto.UsuarioId;
                prestamo.FechaLimite = dto.FechaLimite;
                prestamo.FechaModificacion = dto.FechaMod;
                prestamo.ModificadoPor = dto.UsuarioMod.ToString();


                await _prestamoRepository.UpdateEntityAsync(prestamo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar el prestamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Remove(PrestamoRemoveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(dto.Id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "prestamo no encontrado.";
                    return result;
                }
                prestamo.Estado = dto.Estado;
                await _prestamoRepository.UpdateEntityAsync(prestamo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar el prestamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetVencidosByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _prestamoRepository.GetByUsuarioIdAsync(usuarioId))
                    .Where(p => p.FechaLimite < DateTime.Now && p.Estado == true) // Filtra los préstamos vencidos
                    .Select(p => new PrestamoUpdateDto()
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        LibroId = p.LibroId,
                        FechaLimite = p.FechaLimite,
                        FechaMod = p.FechaCreacion,
                        UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0,
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener los prestamos vencidos del usuario.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _prestamoRepository.GetByUsuarioIdAsync(usuarioId))
                    .Select(p => new PrestamoUpdateDto()
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        LibroId = p.LibroId,
                        FechaLimite = p.FechaLimite,
                        FechaMod = p.FechaCreacion,
                        UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener los prestamos del usuario";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}