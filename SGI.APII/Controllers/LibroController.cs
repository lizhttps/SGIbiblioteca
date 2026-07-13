using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Libros;
using SGI.Application.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _LibroService;

        public LibroController(ILibroService LibroService)
        {
            _LibroService = LibroService;
        }

        [HttpGet("GetLibros")]
        public async Task<IActionResult> GetData()
        {
            var result = await _LibroService.GetData();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("GetLibroByISBN")]
        public async Task<IActionResult> GetByISBN(string isbn)
        {
            var result = await _LibroService.GetByISBN(isbn);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetLibroByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            var result = await _LibroService.GetDataById(id);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("CreateLibro")]
        public async Task<IActionResult> Post([FromBody] LibroSaveDto LibroSaveDto)
        {
            var result = await _LibroService.Save(LibroSaveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("ModifyLibro")]
        public async Task<IActionResult> Post([FromBody] LibroUpdateDto LibroUpdateDto)
        {
            var result = await _LibroService.Update(LibroUpdateDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("DisabledLibro")]
        public async Task<IActionResult> Post([FromBody] LibroRemoveDto LibroRemoveDto)
        {
            var result = await _LibroService.Remove(LibroRemoveDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
