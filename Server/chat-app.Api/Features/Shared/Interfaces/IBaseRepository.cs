using System.Linq.Expressions;

namespace chat_app.Api.Features.Shared.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> UpdateAsync(T entity);
        Task DeleteByIdAsync(Guid id);
    }
}
