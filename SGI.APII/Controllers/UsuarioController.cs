using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Usario;
using SGI.Application.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILoggerService _loggerService;

        public UsuarioController(IUsuarioService usuarioService, ILoggerService loggerService)
        {
            _usuarioService = usuarioService;
            _loggerService = loggerService;
        }

        [HttpGet("GetUsuario")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _usuarioService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de usuarios. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de usuarios.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("GetUsuarioByID")]
        public async Task<IActionResult> GetDataById(int usuarioId)
        {
            try
            {
                var result = await _usuarioService.GetDataById(usuarioId);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró el usuario con id {usuarioId}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar el usuario con id {usuarioId}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("GetCorreoByUsuario")]
        public async Task<IActionResult> GetCorreo(string correo)
        {
            try
            {
                var result = await _usuarioService.GetCorreo(correo);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró usuario con correo {correo}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar usuario con correo {correo}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("CreateUsuario")]
        public async Task<IActionResult> Post([FromBody] UsuarioSaveDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _usuarioService.Save(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear el usuario. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el usuario.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("ModifyUsuario")]
        public async Task<IActionResult> Modify([FromBody] UsuarioUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _usuarioService.Update(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar el usuario con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar el usuario con id {dto.Id}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost("DisabledUsuario")]
        public async Task<IActionResult> Disable([FromBody] UsuarioRemoveDto dto)
        {
            try
            {
                var result = await _usuarioService.Remove(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar el usuario con id {dto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar el usuario con id {dto.Id}.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}