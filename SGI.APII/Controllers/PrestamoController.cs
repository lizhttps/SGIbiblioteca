using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Prestamo;
using SGI.Application.Interfaces;
namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _PrestamoService;
        public PrestamoController(IPrestamoService PrestamoService)
        {
            _PrestamoService = PrestamoService;
        }
        [HttpGet("GetPrestamo")]
        public async Task<IActionResult> GetData()
        {
            var result = await _PrestamoService.GetData();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpGet("GetPrestamoID")]
        public async Task<IActionResult> GetDataById(int prestamoid)
        {
            var result = await _PrestamoService.GetDataById(prestamoid);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpGet("GetPrestamoVencidoByUsuario")]
        public async Task<IActionResult> GetVencidosByUsuarioId(int usuarioId)
        {
            var result = await _PrestamoService.GetVencidosByUsuarioId(usuarioId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpPost("CreatePrestamo")]
        public async Task<IActionResult> Create([FromBody] PrestamoSaveDto dto)
        {
            var result = await _PrestamoService.Save(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpPost("ModifyPrestamo")]
        public async Task<IActionResult> Modify([FromBody] PrestamoUpdateDto dto)
        {
            var result = await _PrestamoService.Update(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpPost("DisabledPrestamo")]
        public async Task<IActionResult> Disable([FromBody] PrestamoRemoveDto dto)
        {
            var result = await _PrestamoService.Remove(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}