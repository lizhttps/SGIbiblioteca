using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;
        private readonly ILoggerService _loggerService;

        public PrestamoController(IPrestamoService prestamoService, ILoggerService loggerService)
        {
            _prestamoService = prestamoService;
            _loggerService = loggerService;
        }

        [HttpGet("GetPrestamo")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _prestamoService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de préstamos. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de préstamos.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener los préstamos."
                });
            }
        }

        [HttpGet("GetPrestamoID")]
        public async Task<IActionResult> GetDataById(int prestamoId)
        {
            try
            {
                var result = await _prestamoService.GetDataById(prestamoId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró el préstamo con id {prestamoId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar el préstamo con id {prestamoId}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar el préstamo."
                });
            }
        }

        [HttpGet("GetPrestamoVencidoByUsuario")]
        public async Task<IActionResult> GetVencidosByUsuarioId(int usuarioId)
        {
            try
            {
                var result = await _prestamoService.GetVencidosByUsuarioId(usuarioId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudieron obtener préstamos vencidos del usuario {usuarioId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar préstamos vencidos del usuario {usuarioId}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar préstamos vencidos del usuario."
                });
            }
        }

        [HttpPost("CreatePrestamo")]
        public async Task<IActionResult> Create([FromBody] PrestamoSaveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _prestamoService.Save(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear el préstamo. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el préstamo.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al crear el préstamo."
                });
            }
        }

        [HttpPost("ModifyPrestamo")]
        public async Task<IActionResult> Modify([FromBody] PrestamoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _prestamoService.Update(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar el préstamo con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar el préstamo con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al modificar el préstamo."
                });
            }
        }

        [HttpPost("DisabledPrestamo")]
        public async Task<IActionResult> Disable([FromBody] PrestamoRemoveDto dto)
        {
            try
            {
                var result = await _prestamoService.Remove(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar el préstamo con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar el préstamo con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al deshabilitar el préstamo."
                });
            }
        }

        [HttpPost("AprobarPrestamo")]
        public async Task<IActionResult> Aprobar([FromBody] PrestamoDecisionDto dto)
        {
            try
            {
                var result = await _prestamoService.AprobarPrestamo(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo aprobar el préstamo con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al aprobar el préstamo con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al aprobar el préstamo."
                });
            }
        }

        [HttpPost("RechazarPrestamo")]
        public async Task<IActionResult> Rechazar([FromBody] PrestamoDecisionDto dto)
        {
            try
            {
                var result = await _prestamoService.RechazarPrestamo(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo rechazar el préstamo con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al rechazar el préstamo con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al rechazar el préstamo."
                });
            }
        }
    }
}
