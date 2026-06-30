using SGIbiblioteca.Domain.Base;
namespace SGI.Application.Base
{
    public interface IBaseService<TDtoSave, TDtoUpdate, TDtoRemove>
    {
        Task<OperationResult> GetData();

        Task<OperationResult> GetDataById(int id);

        Task<OperationResult> Update(TDtoUpdate dto);

        Task<OperationResult> Remove(TDtoRemove dto);

        Task<OperationResult> Save(TDtoSave dto);
    }
}
