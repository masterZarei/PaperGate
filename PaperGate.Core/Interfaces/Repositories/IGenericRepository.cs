using PaperGate.Shared.ReturnTypes;
using System.Linq.Expressions;

namespace PaperGate.Core.Interfaces.Repositories;
public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<TaskResult> AddAsync(T entity);
    Task<TaskResult> RemoveAsync(T entity);
    Task<TaskResult> UpdateAsync(T entity);
    Task<TaskResult> SaveChangesAsync();
}