using SGI.WEB.Models;
using SGI.WEB.Models.Penalizacion;

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
