using System.Linq.Expressions;

namespace Chillgo.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<int> CountAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);

        Task<T?> GetOneAsync(Expression<Func<T, bool>> expression, bool hasTrackings = true);

        Task<T?> GetByIdAsync(Guid id);

        Task AddAsync(T TEntity);

        Task AddRangeAsync(IEnumerable<T> Tentities);

        Task UpdateAsync(T TEntity);

        Task DeleteAsync(T TEntity);

        Task DeleteRangeAsync(IEnumerable<T> TEntities);

        Task<bool> SaveChangeAsync();
    }
}
