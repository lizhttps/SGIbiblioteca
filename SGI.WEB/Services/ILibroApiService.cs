using SGI.WEB.Models;
using SGI.WEB.Models.Libro;

namespace SGI.WEB.Services
{
    public interface ILibroApiService
    {
        Task<ApiResponse<List<LibroEditModel>>> GetLibros();
        Task<ApiResponse<LibroEditModel>> GetLibroById(int id);
        Task<bool> CreateLibro(LibroCreateModel model);
        Task<bool> ModifyLibro(LibroEditModel model);
        Task<bool> DisabledLibro(int id);
    }
}
