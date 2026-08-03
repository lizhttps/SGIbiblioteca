using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Libros;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Interfaces;


namespace SGI.APII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _LibroService;
        private readonly ILoggerService _loggerService;

        public LibroController(ILibroService LibroService, ILoggerService loggerService)
        {
            _LibroService = LibroService;
            _loggerService = loggerService;
        }

        [HttpGet("GetLibros")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = await _LibroService.GetData();
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo obtener la lista de libros. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al obtener la lista de libros.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al obtener la lista de libros."
                });
            }
        }

        [HttpGet("GetLibroByISBN")]
        public async Task<IActionResult> GetByISBN(string isbn)
        {
            try
            {
                var result = await _LibroService.GetByISBN(isbn);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró el libro con ISBN {isbn}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar el libro con ISBN {isbn}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar el libro por ISBN."
                });
            }
        }

        [HttpGet("GetLibroByID")]
        public async Task<IActionResult> GetDataById(int id)
        {
            try
            {
                var result = await _LibroService.GetDataById(id);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se encontró el libro con id {id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al buscar el libro con id {id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al buscar el libro."
                });
            }
        }

        [HttpPost("CreateLibro")]
        public async Task<IActionResult> Create([FromBody] LibroSaveDto LibroSaveDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _LibroService.Save(LibroSaveDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo crear el libro. Mensaje: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error al crear el libro.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al crear el libro."
                });
            }
        }

        [HttpPost("ModifyLibro")]
        public async Task<IActionResult> Modify([FromBody] LibroUpdateDto LibroUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _LibroService.Update(LibroUpdateDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo modificar el libro con id {LibroUpdateDto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al modificar el libro con id {LibroUpdateDto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al modificar el libro."
                });
            }
        }

        [HttpPost("DisabledLibro")]
        public async Task<IActionResult> Disable([FromBody] LibroRemoveDto LibroRemoveDto)
        {
            try
            {
                var result = await _LibroService.Remove(LibroRemoveDto);
                if (result.Success)
                    return Ok(result);

                _loggerService.LogWarning($"No se pudo deshabilitar el libro con id {LibroRemoveDto.Id}.");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al deshabilitar el libro con id {LibroRemoveDto.Id}.");
                return StatusCode(500, new OperationResult
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al deshabilitar el libro."
                });
            }
        }
    }
}
