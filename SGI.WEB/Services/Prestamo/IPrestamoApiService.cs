using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;

namespace SGI.WEB.Services.Prestamo
{
    public interface IPrestamoApiService
    {
        Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamos();
        Task<ApiResponse<PrestamoEditModel>> GetPrestamoById(int id);
        Task<ApiResponse<object>> CreatePrestamo(PrestamoCreateModel model);
        Task<ApiResponse<object>> ModifyPrestamo(PrestamoEditModel model);
        Task<ApiResponse<object>> DisabledPrestamo(int id);
    }
}