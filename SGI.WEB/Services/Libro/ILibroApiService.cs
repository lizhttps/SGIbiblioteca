using SGI.WEB.Models;
using SGI.WEB.Models.Libro;

namespace SGI.WEB.Services.Libro
{
    public interface ILibroApiService
    {
        Task<ApiResponse<List<LibroEditModel>>> GetLibros();
        Task<ApiResponse<LibroEditModel>> GetLibroById(int id);
        Task<ApiResponse<object>> CreateLibro(LibroCreateModel model);
        Task<ApiResponse<object>> ModifyLibro(LibroEditModel model);
        Task<ApiResponse<object>> DisabledLibro(int id);
    }
}
