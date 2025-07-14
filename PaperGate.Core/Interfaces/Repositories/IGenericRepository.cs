using PaperGate.Shared.ReturnTypes;
using System.Linq.Expressions;

namespace PaperGate.Core.Interfaces.Repositories;
public interface IGenericRepository<T> where T : class
{
    Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null);

    Task<IReadOnlyList<T>> GetAllReadOnlyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null);

    Task<T?> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        // bool tracked = true,
        Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null);

    Task<TaskResult> AddAsync(T entity);
    Task<TaskResult> UpdateAsync(T entity);
    Task<TaskResult> RemoveAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task<TaskResult> SaveChangesAsync();
}
