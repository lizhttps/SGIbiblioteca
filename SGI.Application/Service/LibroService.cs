
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGI.Application.Dtos.Libros;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Repositorio;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;


namespace SGI.Application.Service
{
    public class LibroService : ILibroService
    {

        private readonly ILibroRepository _libroRepository;
        private readonly ILogger<LibroService> _logger;
        private readonly IConfiguration _configuration;

        public LibroService(ILibroRepository LibroRepository, ILogger<LibroService> logger, IConfiguration configuration)
        {
            _libroRepository = LibroRepository;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<OperationResult> GetByISBN(string isbn)
        {
            OperationResult result = new OperationResult();

            try
            {
                var libroid = await _libroRepository.GetByISBNAsync(isbn);

                var libroresult = new LibroUpdateDto()
                {
                    Id = libroid.Id,
                    Titulo = libroid.Titulo,
                    Autor = libroid.Autor,
                    ISBN = libroid.ISBN,
                    Categoria = libroid.Categoria,
                    CantidadTotal = libroid.CantidadTotal,
                    CantidadDisponible = libroid.CantidadDisponible,
                    Estado = libroid.Estado,
                    FechaMod = libroid.FechaCreacion,
                    UsuarioMod = int.TryParse(libroid.CreadoPor, out int user) ? user : 0 // usuario realizando la modificacion
                };

                result.Data = libroresult;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Error obteniendo";
                _logger.LogError(result.Message, ex);
            }

            return result;
        }


        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _libroRepository.GetAllAsync())
                    .Select(libro => new LibroUpdateDto()
                    {
                        Id = libro.Id,
                        Titulo = libro.Titulo,
                        Autor = libro.Autor,
                        ISBN = libro.ISBN,
                        Categoria = libro.Categoria,
                        CantidadTotal = libro.CantidadTotal,
                        CantidadDisponible = libro.CantidadDisponible,
                        Estado = libro.Estado,
                        FechaMod = libro.FechaCreacion,
                        UsuarioMod = int.TryParse(libro.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los libros";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var libroid = await _libroRepository.GetEntityByIdAsync(id);

                var libroresult = new LibroUpdateDto()
                {
                    Id = libroid.Id,
                    Titulo = libroid.Titulo,
                    Autor = libroid.Autor,
                    ISBN = libroid.ISBN,
                    Categoria = libroid.Categoria,
                    CantidadTotal = libroid.CantidadTotal,
                    CantidadDisponible = libroid.CantidadDisponible,
                    Estado = libroid.Estado,
                    FechaMod = libroid.FechaCreacion,
                    UsuarioMod = int.TryParse(libroid.CreadoPor, out int user) ? user : 0
                };

                result.Data = libroresult;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Error obteniendo";
                _logger.LogError(result.Message, ex);
            }

            return result;
        }

        public async Task<OperationResult> Remove(LibroRemoveDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var libroToDelete = await _libroRepository.GetEntityByIdAsync(dto.Id);

                if (libroToDelete == null) // si el libro no existe, devolvemos un error.
                {
                    result.Success = false;
                    result.Message = "Libro no encontrado.";
                    return result;
                }
                AuditEntity baseLibro = libroToDelete;
                baseLibro.Estado = dto.Estado; 

               
                await _libroRepository.UpdateEntityAsync(libroToDelete);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al intentar eliminar el libro.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
        

        public async Task<OperationResult> Save(LibroSaveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _libroRepository.SaveEntityAsync(new Libro()
                {

                    Titulo = dto.Titulo,
                    Autor = dto.Autor,
                    ISBN = dto.ISBN,
                    Categoria = dto.Categoria,
                    CantidadTotal = dto.CantidadTotal,
                    CantidadDisponible = dto.CantidadDisponible,
                    Estado = dto.Estado,
                    FechaCreacion = dto.FechaMod,
                    CreadoPor = dto.UsuarioMod.ToString()

                });
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Error guardando el Libro";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }


        public async Task<OperationResult> Update(LibroUpdateDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var libroToUpdate = await _libroRepository.GetEntityByIdAsync(dto.Id);
                libroToUpdate.Titulo = dto.Titulo;
                libroToUpdate.Autor = dto.Autor;
                libroToUpdate.ISBN = dto.ISBN;
                libroToUpdate.Categoria = dto.Categoria;
                libroToUpdate.Estado = dto.Estado;
                libroToUpdate.CantidadTotal = dto.CantidadTotal;
                libroToUpdate.CantidadDisponible = dto.CantidadDisponible;
                libroToUpdate.FechaModificacion = dto.FechaMod;
                libroToUpdate.ModificadoPor = dto.UsuarioMod.ToString();


                await _libroRepository.UpdateEntityAsync(libroToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el Libro";
                _logger.LogError(result.Message, ex);
            }

            return result;
        }
    }
}
