using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Notificacion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _NotificacionService;
        private readonly ILoggerService _loggerService;

        public NotificacionController(INotificacionService NotificacionService, ILoggerService loggerService)
        {
            _NotificacionService = NotificacionService;
            _loggerService = loggerService;
        }

        [HttpGet("GetNotificaciones")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _NotificacionService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de notificaciones. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de notificaciones.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener las notificaciones."
                });
            }
        }

        [HttpGet("GetNotiByUsuario")]
        public async Task<IActionResult> GetNotiUsuario(int noti)
        {
            try
            {
                var result = await _NotificacionService.GetNotiUsuario(noti);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudieron obtener las notificaciones del usuario {noti}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al obtener notificaciones del usuario {noti}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener las notificaciones del usuario."
                });
            }
        }

        [HttpGet("GetNotiByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            try
            {
                var result = await _NotificacionService.GetDataById(id);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró la notificación con id {id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar la notificación con id {id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar la notificación."
                });
            }
        }

        [HttpPost("CreateNoficacion")]
        public async Task<IActionResult> Create([FromBody] NotificacionSaveDto NotificacionSaveDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _NotificacionService.Save(NotificacionSaveDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear la notificación. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear la notificación.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al crear la notificación."
                });
            }
        }

        [HttpPost("ModifyNotificacion")]
        public async Task<IActionResult> Modify([FromBody] NotificacionUpdateDto NotificacionUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _NotificacionService.Update(NotificacionUpdateDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar la notificación con id {NotificacionUpdateDto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar la notificación con id {NotificacionUpdateDto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al modificar la notificación."
                });
            }
        }

        [HttpPost("DisabledNotificacion")]
        public async Task<IActionResult> Disable([FromBody] NotificacionRemoveDto NotificacionRemoveDto)
        {
            try
            {
                var result = await _NotificacionService.Remove(NotificacionRemoveDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar la notificación con id {NotificacionRemoveDto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar la notificación con id {NotificacionRemoveDto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al deshabilitar la notificación."
                });
            }
        }
    }
}
