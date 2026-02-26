using System.Linq.Expressions;

namespace src.features.shared.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> UpdateAsync(TEntity entity);
}
