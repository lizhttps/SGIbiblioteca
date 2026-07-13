using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Penalizacion;
using SGI.Application.Interfaces;

namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenalizacionController : ControllerBase
    {
        private readonly IPenalizacionService _PenalizacionService;

        public PenalizacionController(IPenalizacionService PenalizacionService)
        {
            _PenalizacionService =  PenalizacionService;
        }

        [HttpGet("GetPenalizacion")]
        public async Task<IActionResult> GetData()
        {
            var result = await _PenalizacionService.GetData();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("GetPenalizacionByID")]
        public async Task<IActionResult> GetDataById(int penaid)
        {
            var result = await _PenalizacionService.GetDataById(penaid);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetPencidaByUsuario")]
        public async Task<IActionResult> GetPenalizacionesVencidasByUsuarioId(int usuarioVENid)
        {
            var result = await _PenalizacionService.GetPenalizacionesVencidasByUsuarioId(usuarioVENid);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetPenActByUsuario")]
        public async Task<IActionResult> GetPenalizacionesByUsuarioId(int usuarioACTid)
        {
            var result = await _PenalizacionService.GetPenalizacionesByUsuarioId(usuarioACTid);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CreatePenalizacion")]
        public async Task<IActionResult> Post([FromBody] PenalizacionSaveDto PenalizacionSaveDto)
        {
            var result = await _PenalizacionService.Save(PenalizacionSaveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("ModifyPenalizacion")]
        public async Task<IActionResult> Modify([FromBody] PenalizacionUpdateDto PenalizacionUpdateDto)
        {
            var result = await _PenalizacionService.Update(PenalizacionUpdateDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisabledPenalizacion")]
        public async Task<IActionResult> Put([FromBody] PenalizacionRemoveDto PenalizacionRemoveDto)
        {
            var result = await _PenalizacionService.Remove(PenalizacionRemoveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
