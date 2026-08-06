using SGI.WEB.Models;
using SGI.WEB.Models.Devolucion;
using System.Net.Http.Json;
using SGI.Shared.Models;
namespace SGI.WEB.Services.Devolucion
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
