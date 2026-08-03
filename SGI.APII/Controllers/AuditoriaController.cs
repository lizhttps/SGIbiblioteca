using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Auditoria;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILoggerService _loggerService;

        public AuditoriaController(IAuditoriaService auditoriaService, ILoggerService loggerService)
        {
            _auditoriaService = auditoriaService;
            _loggerService = loggerService;
        }

        [HttpGet("GetAuditorias")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _auditoriaService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de auditorías. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de auditorías.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener las auditorías."
                });
            }
        }

        [HttpGet("GetAuditoriaByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            try
            {
                var result = await _auditoriaService.GetDataById(id);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró la auditoría con id {id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar la auditoría con id {id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar la auditoría."
                });
            }
        }

        [HttpGet("GetAuditByEntidad")]
        public async Task<IActionResult> GetAuditoriaByEntidad([FromQuery] string? entidad)
        {
            try
            {
                var result = await _auditoriaService.GetByEntidad(entidad);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener auditoría para la entidad {entidad}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar auditoría para la entidad {entidad}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar auditoría por entidad."
                });
            }
        }

        [HttpPost("CreateAudit")]
        public async Task<IActionResult> Post([FromBody] AuditoriaSaveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _auditoriaService.Save(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear el registro de auditoría. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el registro de auditoría.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al crear el registro de auditoría."
                });
            }
        }
    }
}
