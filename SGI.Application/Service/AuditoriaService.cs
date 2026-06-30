using Microsoft.Extensions.Logging;
using SGI.Application.Dtos.Auditoria;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entities.Auditorias;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{
        public class AuditoriaService : IAuditoriaService
        {
            private readonly IAuditoriaRepository _auditoriaRepository;
            private readonly ILogger<AuditoriaService> _logger;
            public AuditoriaService(IAuditoriaRepository auditoriaRepository, ILogger<AuditoriaService> logger)
            {
                _auditoriaRepository = auditoriaRepository;
                _logger = logger;
            }
        public async Task<OperationResult> Save(AuditoriaSaveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _auditoriaRepository.SaveEntityAsync(new Auditoria()
                {
                    Entidad = dto.Entidad,
                    Accion = dto.Accion,
                    Detalle = dto.Detalle,
                    FechaCreacion = DateTime.Now,
                });  
            }    
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al registrar la auditoría.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }


        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _auditoriaRepository.GetAllAsync())
                    .Select(auditoria => new AuditoriaSaveDto()
                    {
                        Entidad = auditoria.Entidad,
                        Accion = auditoria.Accion,
                        Detalle = auditoria.Detalle,
                        EntidadId = auditoria.EntidadId,
                        RealizadoPor = auditoria.RealizadoPor,
                    }).ToList();  
            } 
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener los registros de auditoría.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }


        public async Task<OperationResult> GetDataById(int id)
            {
                OperationResult result = new OperationResult();
            try
            {
                var audiid = await _auditoriaRepository.GetEntityByIdAsync(id);

                if (audiid == null) // si el registro no existe, devolvemos un error.
                {
                    result.Success = false;
                    result.Message = "Registro no encontrado.";
                    return result;
                }
                var auditoriaresult = new AuditoriaSaveDto()
                {
                    Entidad = audiid.Entidad,
                    Accion = audiid.Accion,
                    Detalle = audiid.Detalle,
                    EntidadId = audiid.EntidadId,
                    RealizadoPor = audiid.RealizadoPor,

                };

                result.Data = auditoriaresult;
            }

            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al obtener el registro de auditoría.";
                _logger.LogError(ex, result.Message);
            }
                return result;
            }

            
            public async Task<OperationResult> GetByEntidad(string entidad)
            {
                OperationResult result = new OperationResult();
                try
                {
                result.Data = (await _auditoriaRepository.GetByEntidadAsync(entidad))
                .Select(auditoria => new AuditoriaSaveDto()
                {
                    Entidad = auditoria.Entidad,
                    Accion = auditoria.Accion,
                    Detalle = auditoria.Detalle,
                    EntidadId = auditoria.EntidadId,
                    RealizadoPor = auditoria.RealizadoPor,
                }).ToList();
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = "Error al obtener la auditoría por entidad.";
                    _logger.LogError(ex, result.Message);
                }
                return result;
            }
        }
    }
