using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGI.Application.Dtos.Devolucion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Devoluciones;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{
    public class DevolucionService : IDevolucionService
    {
        private readonly IDevolucionRepository _devolucionRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ILibroRepository _libroRepository;
        private readonly ILogger<DevolucionService> _logger;
        private readonly IConfiguration _configuration;

        public DevolucionService(
            IDevolucionRepository devolucionRepository,
            IPrestamoRepository prestamoRepository,
            ILibroRepository libroRepository,
            ILogger<DevolucionService> logger,
            IConfiguration configuration)
        {
            _devolucionRepository = devolucionRepository;
            _prestamoRepository = prestamoRepository;
            _libroRepository = libroRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _devolucionRepository.GetAllAsync())
                    .Select(d => new DevolucionUpdateDto()
                    {
                        Id = d.Id,
                        PrestamoId = d.PrestamoId,
                        Observaciones = d.Observaciones,
                        DevueltoATiempo = d.DevueltoATiempo,
                        FechaDevolucion = d.FechaDevolucion,
                        FechaMod = d.FechaCreacion,
                        UsuarioMod = int.TryParse(d.CreadoPor, out int user) ? user : 0

                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las devoluciones.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var devolucionid = await _devolucionRepository.GetEntityByIdAsync(id);
                if (devolucionid == null)
                {
                    result.Success = false;
                    result.Message = "Devolución no encontrada.";
                    return result;
                }
                result.Data = new DevolucionUpdateDto()
                {
                    Id = devolucionid.Id,
                    PrestamoId = devolucionid.PrestamoId,
                    DevueltoATiempo = devolucionid.DevueltoATiempo,
                    Observaciones = devolucionid.Observaciones,
                    FechaMod = devolucionid.FechaCreacion,
                    UsuarioMod = int.TryParse(devolucionid.CreadoPor, out int user) ? user : 0
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener la devolución.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Save(DevolucionSaveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _devolucionRepository.SaveEntityAsync(new Devolucion()
                {
                    PrestamoId = dto.PrestamoId,
                    Estado = true,
                    Observaciones = dto.Observaciones,
                    FechaCreacion = dto.FechaMod,
                    CreadoPor = dto.UsuarioMod.ToString(),
                });
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al registrar la devolución.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Update(DevolucionUpdateDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var devolucion = await _devolucionRepository.GetEntityByIdAsync(dto.Id);
                if (devolucion == null)
                {
                    result.Success = false;
                    result.Message = "Devolución no encontrada.";
                    return result;
                }

                devolucion.Observaciones = dto.Observaciones;
                devolucion.FechaModificacion = dto.FechaMod;
                devolucion.ModificadoPor = dto.UsuarioMod.ToString();
                devolucion.DevueltoATiempo = dto.DevueltoATiempo;


                await _devolucionRepository.UpdateEntityAsync(devolucion);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar la devolución.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Remove(DevolucionRemoveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var devolucion = await _devolucionRepository.GetEntityByIdAsync(dto.Id);
                if (devolucion == null)
                {
                    result.Success = false;
                    result.Message = "Devolución no encontrada.";
                    return result;
                }
                devolucion.Estado = dto.Estado;
                await _devolucionRepository.UpdateEntityAsync(devolucion);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al eliminar la devolución.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetByPrestamoId(int prestamoId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var devolucion = await _devolucionRepository.GetByPrestamoIdAsync(prestamoId);
                if (devolucion == null)
                {
                    result.Success = false;
                    result.Message = "No existe una devolución registrada para este préstamo.";
                    return result;
                }

                result.Data = new DevolucionUpdateDto()
                {
                    Id = devolucion.Id,
                    PrestamoId = prestamoId,
                    DevueltoATiempo = devolucion.DevueltoATiempo,
                    Observaciones = devolucion.Observaciones,
                    FechaMod = devolucion.FechaCreacion,
                    UsuarioMod = int.TryParse(devolucion.CreadoPor, out int user) ? user : 0 
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener la devolución por préstamo.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        // ARREGLAR METODO
        public async Task<OperationResult> GetDevolucionesByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                // 1. Traemos los préstamos del usuario
                var prestamosUsuario = await _prestamoRepository.GetByUsuarioIdAsync(usuarioId);
                var prestamosIds = prestamosUsuario.Select(p => p.Id).ToList();

                // 2. Traemos las devoluciones que correspondan a esos prestamos
                result.Data = (await _devolucionRepository.GetAllAsync())
                    .Where(d => prestamosIds.Contains(d.PrestamoId))
                    .Select(d => new DevolucionUpdateDto()
                    {
                        Id = d.Id,
                        PrestamoId = d.PrestamoId,
                        Observaciones = d.Observaciones,
                        DevueltoATiempo = d.DevueltoATiempo,
                        FechaDevolucion = d.FechaDevolucion,
                        FechaMod = d.FechaCreacion,
                        UsuarioMod = int.TryParse(d.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las devoluciones del usuario.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

    }
}

