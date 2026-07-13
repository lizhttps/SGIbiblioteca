using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Devolucion;
using SGI.Application.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionController : ControllerBase
    {
        private readonly IDevolucionService _devolucionService;

        public DevolucionController(IDevolucionService devolucionService)
        {
            _devolucionService = devolucionService;
        }

        [HttpGet("GetDevoluciones")]
        public async Task<IActionResult> GetData()
        {
            var result = await _devolucionService.GetData();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetDevolucionByID")]
        public async Task<IActionResult> GetDataById(int devoid)
        {
            var result = await _devolucionService.GetDataById(devoid);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetDevolucionesByUsuario")]
        public async Task<IActionResult> GetDevolucionesByUsuarioId(int usuarioId)
        {
            var result = await _devolucionService.GetDevolucionesByUsuarioId(usuarioId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetDevolucionByPrestamo")]
        public async Task<IActionResult> GetByPrestamoId(int prestamoid)
        {
            var result = await _devolucionService.GetByPrestamoId(prestamoid);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CreateDevolucion")]
        public async Task<IActionResult> Create([FromBody] DevolucionSaveDto dto)
        {
            var result = await _devolucionService.Save(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ModifyDevolucion")]
        public async Task<IActionResult> Modify([FromBody] DevolucionUpdateDto dto)
        {
            var result = await _devolucionService.Update(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisabledDevolucion")]
        public async Task<IActionResult> Disable([FromBody] DevolucionRemoveDto dto)
        {
            var result = await _devolucionService.Remove(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
