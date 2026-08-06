using SGI.Application.Dtos.Prestamo;
using SGI.WEB.Models;
using SGI.WEB.Models.Prestamo;
using SGIbiblioteca.Domain.Base;

namespace SGI.WEB.Services.Prestamo
{
    public interface IPrestamoApiService
    {
        Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamos();
        Task<ApiResponse<List<PrestamoEditModel>>> GetPrestamosByUsuario(int usuarioId);
        Task<ApiResponse<PrestamoEditModel>> GetPrestamoById(int id);
        Task<ApiResponse<object>> CreatePrestamo(PrestamoCreateModel model);
        Task<ApiResponse<object>> ModifyPrestamo(PrestamoEditModel model);
        Task<ApiResponse<object>> DisabledPrestamo(int id);
        Task<ApiResponse<object>> AprobarPrestamo(int id, int usuarioMod, DateTime fechaDevolucion); 
        Task<ApiResponse<object>> RechazarPrestamo(int id, int usuarioMod);
    }
}
