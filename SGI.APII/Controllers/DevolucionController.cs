using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Devolucion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionController : ControllerBase
    {
        private readonly IDevolucionService _devolucionService;
        private readonly ILoggerService _loggerService;

        public DevolucionController(IDevolucionService devolucionService, ILoggerService loggerService)
        {
            _devolucionService = devolucionService;
            _loggerService = loggerService;
        }

        [HttpGet("GetDevoluciones")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _devolucionService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de devoluciones. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de devoluciones.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("GetDevolucionByID")]
        public async Task<IActionResult> GetDataById(int devoId)
        {
            try
            {
                var result = await _devolucionService.GetDataById(devoId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró la devolución con id {devoId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar la devolución con id {devoId}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("GetDevolucionesByUsuario")]
        public async Task<IActionResult> GetDevolucionesByUsuarioId(int usuarioId)
        {
            try
            {
                var result = await _devolucionService.GetDevolucionesByUsuarioId(usuarioId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudieron obtener devoluciones del usuario {usuarioId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar devoluciones del usuario {usuarioId}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("GetDevolucionByPrestamo")]
        public async Task<IActionResult> GetByPrestamoId(int prestamoId)
        {
            try
            {
                var result = await _devolucionService.GetByPrestamoId(prestamoId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró devolución para el préstamo {prestamoId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar devolución para el préstamo {prestamoId}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("CreateDevolucion")]
        public async Task<IActionResult> Create([FromBody] DevolucionSaveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _devolucionService.Save(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo registrar la devolución. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al registrar la devolución.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("ModifyDevolucion")]
        public async Task<IActionResult> Modify([FromBody] DevolucionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _devolucionService.Update(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar la devolución con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar la devolución con id {dto.Id}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("DisabledDevolucion")]
        public async Task<IActionResult> Disable([FromBody] DevolucionRemoveDto dto)
        {
            try
            {
                var result = await _devolucionService.Remove(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar la devolución con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar la devolución con id {dto.Id}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}