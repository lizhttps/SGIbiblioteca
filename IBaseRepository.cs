using SGIbiblioteca.Domain.Base;
using System.Linq.Expressions;

namespace SGIbiblioteca.Domain.Repositorio
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetEntityByIdAsync(int id);
        Task UpdateEntityAsync(TEntity entity);
        Task DeleteEntityAsync(TEntity entity);
        Task SaveEntityAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<OperactionResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}
