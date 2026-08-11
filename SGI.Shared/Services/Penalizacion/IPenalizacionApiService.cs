using SGI.WEB.Models;
using SGI.WEB.Models.Penalizacion;
using System.Net.Http.Json;
using SGI.Shared.Models;

namespace SGI.WEB.Services.Penalizacion
{
    public interface IPenalizacionApiService
    {
        Task<ApiResponse<List<PenalizacionEditModel>>> GetPenalizaciones();
        Task<ApiResponse<PenalizacionEditModel>> GetPenalizacionById(int id);
        Task<bool> CreatePenalizacion(PenalizacionCreateModel model);
        Task<bool> ModifyPenalizacion(PenalizacionEditModel model);
        Task<bool> DisabledPenalizacion(int id);
    }
}
