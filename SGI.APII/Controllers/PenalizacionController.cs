using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Penalizacion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenalizacionController : ControllerBase
    {
        private readonly IPenalizacionService _penalizacionService;
        private readonly ILoggerService _loggerService;

        public PenalizacionController(IPenalizacionService penalizacionService, ILoggerService loggerService)
        {
            _penalizacionService = penalizacionService;
            _loggerService = loggerService;
        }

        [HttpGet("GetPenalizacion")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _penalizacionService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de penalizaciones. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de penalizaciones.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener las penalizaciones."
                });
            }
        }

        [HttpGet("GetPenalizacionByID")]
        public async Task<IActionResult> GetDataById(int penaId)
        {
            try
            {
                var result = await _penalizacionService.GetDataById(penaId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró la penalización con id {penaId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar la penalización con id {penaId}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar la penalización."
                });
            }
        }

        [HttpGet("GetPencidaByUsuario")]
        public async Task<IActionResult> GetPenalizacionesVencidasByUsuarioId(int usuarioId)
        {
            try
            {
                var result = await _penalizacionService.GetPenalizacionesVencidasByUsuarioId(usuarioId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudieron obtener penalizaciones vencidas del usuario {usuarioId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar penalizaciones vencidas del usuario {usuarioId}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar penalizaciones vencidas del usuario."
                });
            }
        }

        [HttpGet("GetPenActByUsuario")]
        public async Task<IActionResult> GetPenalizacionesByUsuarioId(int usuarioId)
        {
            try
            {
                var result = await _penalizacionService.GetPenalizacionesByUsuarioId(usuarioId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudieron obtener penalizaciones activas del usuario {usuarioId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar penalizaciones activas del usuario {usuarioId}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar penalizaciones activas del usuario."
                });
            }
        }

        [HttpPost("CreatePenalizacion")]
        public async Task<IActionResult> Post([FromBody] PenalizacionSaveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _penalizacionService.Save(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear la penalización. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear la penalización.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al crear la penalización."
                });
            }
        }

        [HttpPost("ModifyPenalizacion")]
        public async Task<IActionResult> Modify([FromBody] PenalizacionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _penalizacionService.Update(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar la penalización con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar la penalización con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al modificar la penalización."
                });
            }
        }

        [HttpPost("DisabledPenalizacion")]
        public async Task<IActionResult> Disable([FromBody] PenalizacionRemoveDto dto)
        {
            try
            {
                var result = await _penalizacionService.Remove(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar la penalización con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar la penalización con id {dto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al deshabilitar la penalización."
                });
            }
        }
    }
}
