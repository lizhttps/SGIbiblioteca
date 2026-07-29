using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;

namespace SGI.WEB.Services
{
    public interface IPrestamoApiService
    {
        Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamos();
        Task<ApiResponse<PrestamoEditModel>> GetPrestamoById(int id);
        Task<bool> CreatePrestamo(PrestamoCreateModel model);
        Task<bool> ModifyPrestamo(PrestamoEditModel model);
        Task<bool> DisabledPrestamo(int id);
    }
}
