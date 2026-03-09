using System.Linq.Expressions;

namespace chat_app.Api.Features.Shared.Interfaces;

public interface IBaseRepository<T>
{
    Task<IResult> AddAsync(T entity);
    Task<IResult> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IResult> UpdateAsync(T entity);
}
