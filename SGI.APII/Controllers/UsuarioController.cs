using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Usario;
using SGI.Application.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;

        public UsuarioController(IUsuarioService UsuarioService)
        {
            _UsuarioService =  UsuarioService;
        }

        [HttpGet("GetUsuario")]
        public async Task<IActionResult> GetData()
        {
            var result = await _UsuarioService.GetData();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("GetUsuarioByID")]
        public async Task<IActionResult> GetDataById(int usuarioid)
        {
            var result = await _UsuarioService.GetDataById(usuarioid);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetCorreoByUsuario")]
        public async Task<IActionResult> GetCorreo(string correo)
        {
            var result = await _UsuarioService.GetCorreo(correo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CreateUsuario")]
        public async Task<IActionResult> Post([FromBody] UsuarioSaveDto UsuarioSaveDto)
        {
            var result = await _UsuarioService.Save(UsuarioSaveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("ModifyUsuario")]
        public async Task<IActionResult> Put([FromBody] UsuarioUpdateDto UsuarioUpdateDto)
        {
            var result = await _UsuarioService.Update(UsuarioUpdateDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisabledUsuario")]
        public async Task<IActionResult> Put([FromBody] UsuarioRemoveDto UsuarioRemoveDto)
        {
            var result = await _UsuarioService.Remove(UsuarioRemoveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
