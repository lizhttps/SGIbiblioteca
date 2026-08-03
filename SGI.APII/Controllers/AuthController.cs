using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Auth;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILoggerService _loggerService;

        public AuthController(IAuthService authService, ILoggerService loggerService)
        {
            _authService = authService;
            _loggerService = loggerService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.LoginAsync(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"Intento de login fallido para el correo: {dto.Correo}. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error inesperado al intentar iniciar sesión con el correo: {dto.Correo}");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al iniciar sesión."
                });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.RegisterAsync(dto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"Intento de registro fallido para el correo: {dto.Correo}. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error inesperado al registrar el usuario con correo: {dto.Correo}");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al registrar el usuario."
                });
            }
        }
    }
}
