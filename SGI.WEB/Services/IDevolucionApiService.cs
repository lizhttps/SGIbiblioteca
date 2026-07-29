using SGI.WEB.Models;
using SGI.WEB.Models.Devolucion;

namespace SGI.WEB.Services
{
    public interface IDevolucionApiService
    {
        Task<ApiResponse<List<DevolucionEditModel>>> GetDevoluciones();
        Task<ApiResponse<DevolucionEditModel>> GetDevolucionById(int id);
        Task<bool> CreateDevolucion(DevolucionCreateModel model);
        Task<bool> ModifyDevolucion(DevolucionEditModel model);
        Task<bool> DisabledDevolucion(int id);
    }
}
