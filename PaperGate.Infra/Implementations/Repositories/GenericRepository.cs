using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Infra.Data;
using PaperGate.Shared.ReturnTypes;
using Serilog;
using System.Linq.Expressions;

namespace PaperGate.Infra.Implementations.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly ILogger _logger;
    internal DbSet<T> _dbSet;

    public GenericRepository(AppDbContext db, ILogger logger)
    {
        _db = db;
        _logger = logger;
        _dbSet = _db.Set<T>();
    }

    /*public async Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (filter != null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }*/
    public async Task<IList<T>> GetAllAsync(
    Expression<Func<T, bool>>? filter = null,
    Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (queryCustomizer != null)
            query = queryCustomizer(query);

        return await query.ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAllReadOnlyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (queryCustomizer != null)
            query = queryCustomizer(query).AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(
    Expression<Func<T, bool>>? filter = null,
    Func<IQueryable<T>, IQueryable<T>>? queryCustomizer = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (queryCustomizer != null)
            query = queryCustomizer(query);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TaskResult> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            return new TaskResult
            {
                Succeeded = true,
            };
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error in AddAsync");
            return new TaskResult
            {
                Errors = ["خطا در افزودن موجودیت"]
            };
        }
    }

    public Task<TaskResult> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return Task.FromResult(new TaskResult
            {
                Succeeded = true,
            });
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error in UpdateAsync");
            return Task.FromResult(new TaskResult
            {
                Errors = ["خطا در ویرایش موجودیت"]
            });
        }
    }

    public Task<TaskResult> RemoveAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);

            return Task.FromResult(new TaskResult
            {
                Succeeded = true,
            });
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error in RemoveAsync");
            return Task.FromResult(new TaskResult
            {
                Errors = ["خطا در حذف موجودیت"]
            });
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        if (filter is null)
            throw new ArgumentNullException(nameof(filter));

        return await _dbSet.AnyAsync(filter);
    }

    public async Task<TaskResult> SaveChangesAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
            return new TaskResult
            {
                Succeeded = true,
            };
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error in SaveChangesAsync");
            return new TaskResult
            {
                Errors = ["خطا در ذخیره تغییرات"]
            };
        }
    }
}