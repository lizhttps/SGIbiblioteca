using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Auditoria;
using SGI.Application.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _AuditoriaService;

        public AuditoriaController(IAuditoriaService AuditoriaService)
        {
            _AuditoriaService = AuditoriaService;
        }



        [HttpGet("GetAuditorias")]
        public async Task<IActionResult> GetData()
        {
            var result = await _AuditoriaService.GetData();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpGet("GetAuditoriaByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            var result = await _AuditoriaService.GetDataById(id);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAuditByEntidad")]
        public async Task<IActionResult> GetAuditoriaByEntidad([FromQuery] string? entidad)
        {
            var result = await _AuditoriaService.GetByEntidad(entidad);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }



        [HttpPost("CreateAudit")]
        public async Task<IActionResult> Post([FromBody] AuditoriaSaveDto AuditoriaSaveDto)
        {
            var result = await _AuditoriaService.Save(AuditoriaSaveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}