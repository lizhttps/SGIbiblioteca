using SGI.Application.Base;
using SGI.Application.Dtos.Libros;
using SGIbiblioteca.Domain.Base;

namespace SGI.Application.Interfaces
{
    public interface ILibroService : IBaseService<LibroSaveDto, LibroUpdateDto, LibroRemoveDto>
    {
        Task<OperationResult> GetByISBN(string isbn);
    }
}
