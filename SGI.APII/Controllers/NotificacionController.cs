using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Libros;
using SGI.Application.Dtos.Notificacion;
using SGI.Application.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _NotificacionService;

        public NotificacionController(INotificacionService NotificacionService)
        {
            _NotificacionService =  NotificacionService;
        }

        [HttpGet("GetNotificaciones")]
        public async Task<IActionResult> GetData()
        {
            var result = await _NotificacionService.GetData();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("GetNotiByUsuario")]
        public async Task<IActionResult> GetNotiUsuario(int noti)
        {
            var result = await _NotificacionService.GetNotiUsuario(noti);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetNotiByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            var result = await _NotificacionService.GetDataById(id);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CreateNoficacion")]
        public async Task<IActionResult> Post([FromBody] NotificacionSaveDto NotificacionSaveDto)
        {
            var result = await _NotificacionService.Save(NotificacionSaveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("ModifyNotificacion")]
        public async Task<IActionResult> Put([FromBody] NotificacionUpdateDto NotificacionUpdateDto)
        {
            var result = await _NotificacionService.Update(NotificacionUpdateDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisabledNotificacion")]
        public async Task<IActionResult> Put([FromBody] NotificacionRemoveDto NotificacionRemoveDto)
        {
            var result = await _NotificacionService.Remove(NotificacionRemoveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
