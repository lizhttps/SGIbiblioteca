using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGI.Application.Dtos.Penalizacion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entities.Penalizaciones;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{

        public class PenalizacionService : IPenalizacionService
        {
            private readonly IPenalizacionRepository _penalizacionRepository;
            private readonly ILogger<PenalizacionService> _logger;
            private readonly IConfiguration _configuration;

            public PenalizacionService(IPenalizacionRepository penalizacionRepository, ILogger<PenalizacionService> logger, IConfiguration configuration)
            {
                _penalizacionRepository = penalizacionRepository;
                _logger = logger;
                _configuration = configuration;
            }

        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _penalizacionRepository.GetAllAsync())
                    .Select(p => new PenalizacionUpdateDto()
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        Motivo = p.Motivo,
                        Monto = p.Monto,
                        Pagada = p.Pagada,
                        FechaMod = p.FechaCreacion,
                        UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las penalizaciones.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }


        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var penalizacion = await _penalizacionRepository.GetEntityByIdAsync(id);
                if (penalizacion == null)
                {
                    result.Success = false;
                    result.Message = "Penalización no encontrada.";
                    return result;
                }
                result.Data = new PenalizacionUpdateDto()
                {
                    Id = penalizacion.Id,
                    UsuarioId = penalizacion.UsuarioId,
                    Motivo = penalizacion.Motivo,
                    Monto = penalizacion.Monto,
                    Pagada = penalizacion.Pagada,
                    FechaMod = penalizacion.FechaCreacion,
                    UsuarioMod = int.TryParse(penalizacion.CreadoPor, out int user) ? user : 0
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener la penalización.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }


        public async Task<OperationResult> Save(PenalizacionSaveDto dto)
            {
                OperationResult result = new OperationResult();
                try
                {
                result = await _penalizacionRepository.SaveEntityAsync(new Penalizacion()
                {
                    UsuarioId = dto.UsuarioId,
                    Motivo = dto.Motivo,
                    Monto = dto.Monto,
                    FechaCreacion = dto.FechaMod,
                    Estado = true,
                    Pagada = false,
                    CreadoPor = dto.UsuarioMod.ToString()
                });
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = "Error al registrar la penalización.";
                    _logger.LogError(ex, result.Message);
                }
                return result;
            }

        public async Task<OperationResult> Update(PenalizacionUpdateDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var penalizacion = await _penalizacionRepository.GetEntityByIdAsync(dto.Id);
                if (penalizacion == null)
                {
                    result.Success = false;
                    result.Message = "Penalizacion no encontrada.";
                    return result;
                }

                penalizacion.Motivo = dto.Motivo;
                penalizacion.Monto = dto.Monto;
                penalizacion.UsuarioId = dto.UsuarioId;
                penalizacion.Pagada = dto.Pagada;
                penalizacion.ModificadoPor = dto.UsuarioMod.ToString();
                penalizacion.FechaModificacion = dto.FechaMod;

                await _penalizacionRepository.UpdateEntityAsync(penalizacion);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al actualizar la penalizacion.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        // ARREGLAR METODOD REMOVE.
        public async Task<OperationResult> Remove(PenalizacionRemoveDto dto)
            {
                OperationResult result = new OperationResult();
                try
                {
                    var penalizacion = await _penalizacionRepository.GetEntityByIdAsync(dto.Id);
                    if (penalizacion == null)
                    {
                        result.Success = false;
                        result.Message = "Penalización no encontrada.";
                        return result;
                    }
                    penalizacion.Estado = dto.Estado;
                    await _penalizacionRepository.UpdateEntityAsync(penalizacion);
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la penalización.";
                    _logger.LogError(ex, result.Message);
                }
                return result;
            }

            public async Task<OperationResult> GetPenalizacionesByUsuarioId(int usuarioId)
            {
                OperationResult result = new OperationResult();
                try
                {
                    result.Data = (await _penalizacionRepository.GetByUsuarioIdAsync(usuarioId))
                        .Select(p => new PenalizacionUpdateDto()
                        {
                            Id = p.Id,
                            UsuarioId = p.UsuarioId,
                            Motivo = p.Motivo,
                            Monto = p.Monto,
                            Pagada = p.Pagada,
                            FechaMod = p.FechaCreacion,
                            UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0

                        }).ToList();
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = "Error al obtener las penalizaciones del usuario.";
                    _logger.LogError(ex, result.Message);
                }
                return result;
            }

        // GetPenalizacionesVencidasByUsuarioId filtra solo por !p.Pagada
        public async Task<OperationResult> GetPenalizacionesVencidasByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _penalizacionRepository.GetByUsuarioIdAsync(usuarioId))
                    .Where(p => !p.Pagada)
                    .Select(p => new PenalizacionUpdateDto()
                    {
                        Id = p.Id,
                        UsuarioId = p.UsuarioId,
                        Motivo = p.Motivo,
                        Monto = p.Monto,
                        Pagada = p.Pagada,
                        FechaMod = p.FechaCreacion,
                        UsuarioMod = int.TryParse(p.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener las penalizaciones activas del usuario.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

    }
}
