using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;

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
                return StatusCode(500, "Ocurrió un error inesperado.");
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
                return StatusCode(500, "Ocurrió un error inesperado.");
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
                return StatusCode(500, "Ocurrió un error inesperado.");
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
                return StatusCode(500, "Ocurrió un error inesperado.");
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
                return StatusCode(500, "Ocurrió un error inesperado.");
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
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}