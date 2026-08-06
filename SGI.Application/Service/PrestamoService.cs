using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;
using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;
using SGIbiblioteca.Domain.Repositorio;
using SGIbiblioteca.Domain.Interfaces;


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
                var prestamos = (await _prestamoRepository.GetAllAsync())
                    .Where(p => p.Estado)
                    .ToList();

                var libros = (await _libroRepository.GetAllAsync())
                    .ToDictionary(l => l.Id, l => l.Titulo);

                var usuarios = (await _usuarioRepository.GetAllAsync())
                    .ToDictionary(u => u.Id, u => $"{u.Nombre} {u.Apellido}");

                result.Data = prestamos.Select(p => new PrestamoDto
                {
                    Id = p.Id,
                    LibroId = p.LibroId,
                    UsuarioId = p.UsuarioId,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaLimite = p.FechaLimite,
                    EstadoPrestamo = p.EstadoPrestamo,
                    TituloLibro = libros.TryGetValue(p.LibroId, out var titulo) ? titulo : "Libro no disponible",
                    NombreUsuario = usuarios.TryGetValue(p.UsuarioId, out var nombre) ? nombre : "Usuario no disponible"
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
            try
            {
                var usuario = await _usuarioRepository.GetEntityByIdAsync(dto.UsuarioId);
                if (usuario == null || usuario.Estado == false)
                {
                    result.Success = false;
                    result.Message = "El usuario no es válido o está inactivo";
                    return result;
                }

                var penalizaciones = await _penalizacionRepository.GetByUsuarioIdAsync(dto.UsuarioId);
                if (penalizaciones != null && penalizaciones.Any(p => p.Pagada == false))
                {
                    result.Success = false;
                    result.Message = "El usuario tiene penalizaciones pendientes de pago";
                    return result;
                }

                var libro = await _libroRepository.GetEntityByIdAsync(dto.LibroId);
                if (libro == null)
                {
                    result.Success = false;
                    result.Message = "El libro solicitado no existe";
                    return result;
                }

                if (libro.CantidadDisponible <= 0)
                {
                    result.Success = false;
                    result.Message = "No hay copias disponibles de este libro";
                    return result;
                }

                // NO se descuenta stock aquí. Solo se registra la solicitud como Pendiente.
                // El descuento ocurre en AprobarPrestamo().
                result = await _prestamoRepository.SaveEntityAsync(new Prestamo()
                {
                    LibroId = dto.LibroId,
                    UsuarioId = dto.UsuarioId,
                    FechaPrestamo = DateTime.Now,
                    FechaLimite = dto.FechaLimite,
                    FechaCreacion = dto.FechaMod,
                    CreadoPor = dto.UsuarioMod.ToString(),
                    Estado = true,
                    EstadoPrestamo = "Pendiente"
                });
            }
            catch (Exception ex)
            {
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
                var updateResult = await _prestamoRepository.UpdateEntityAsync(prestamo);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message ?? "Error al actualizar el prestamo.";
                    return result;
                }
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
                var updateResult = await _prestamoRepository.UpdateEntityAsync(prestamo);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message ?? "Error al eliminar el prestamo.";
                    return result;
                }
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
                    .Where(p => p.Estado)
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
        public async Task<OperationResult> AprobarPrestamo(PrestamoDecisionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(dto.Id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "Préstamo no encontrado.";
                    return result;
                }

                if (prestamo.EstadoPrestamo != "Pendiente")
                {
                    result.Success = false;
                    result.Message = "Solo se pueden aprobar solicitudes en estado Pendiente.";
                    return result;
                }

                var libro = await _libroRepository.GetEntityByIdAsync(prestamo.LibroId);
                if (libro == null || libro.CantidadDisponible <= 0)
                {
                    result.Success = false;
                    result.Message = "No hay copias disponibles de este libro.";
                    return result;
                }

                // Descontamos el stock solo al aprobar
                libro.CantidadDisponible -= 1;
                await _libroRepository.UpdateEntityAsync(libro);

                prestamo.EstadoPrestamo = "Aprobado";
                prestamo.FechaModificacion = DateTime.Now;
                prestamo.ModificadoPor = dto.UsuarioMod.ToString();
                if (dto.FechaDevolucion.HasValue)
                {
                    prestamo.FechaLimite = dto.FechaDevolucion.Value;
                }

                var updateResult = await _prestamoRepository.UpdateEntityAsync(prestamo);
                if (!updateResult.Success)
                {
                    // revertimos el descuento si falla el update
                    libro.CantidadDisponible += 1;
                    await _libroRepository.UpdateEntityAsync(libro);

                    result.Success = false;
                    result.Message = updateResult.Message ?? "Error al aprobar el préstamo.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al aprobar el préstamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> RechazarPrestamo(PrestamoDecisionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(dto.Id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "Préstamo no encontrado.";
                    return result;
                }

                if (prestamo.EstadoPrestamo != "Pendiente")
                {
                    result.Success = false;
                    result.Message = "Solo se pueden rechazar solicitudes en estado Pendiente.";
                    return result;
                }

                prestamo.EstadoPrestamo = "Rechazado";
                prestamo.FechaModificacion = DateTime.Now;
                prestamo.ModificadoPor = dto.UsuarioMod.ToString();

                var updateResult = await _prestamoRepository.UpdateEntityAsync(prestamo);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message ?? "Error al rechazar el préstamo.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al rechazar el préstamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}