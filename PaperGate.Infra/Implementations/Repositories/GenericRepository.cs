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
    private readonly ILogger _myLogger;
    internal DbSet<T> _dbSet;
    readonly TaskResult _taskResult;

    public GenericRepository(AppDbContext db, ILogger myLogger)
    {
        _db = db;
        _myLogger = myLogger;
        _dbSet = _db.Set<T>();
        _taskResult = new();
    }

    public async Task<TaskResult> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            _taskResult.Succeeded = true;
            return _taskResult;

        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, _dbSet.ToQueryString());
            _taskResult.AddError("در فرآیند اضافه کردن این موجودیت خطایی رخ داد");
            _taskResult.AddError("جهت مشاهده اطلاعات کامل تر به بخش گزارشات مراجعه کنید");
            return _taskResult;
        }

    }

    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
    {
        if (_db.Set<T>() is null)
            throw new NullReferenceException(nameof(T));

        IQueryable<T>? query = _dbSet;
        if (tracked is false)
            query = query.AsNoTracking();

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        if (filter is not null)
            query = query.Where(filter);


        return await query.FirstOrDefaultAsync();
    }
    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        if (filter != null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task<TaskResult> RemoveAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            //Because The deleted entity will be logged at the DbContext Level
            //  await _myLogger.Log("GenericRepository/RemoveAsync", $"Entity deleted : {JsonSerializer.Serialize(entity)}", LoggingLevel.Warning);
            _taskResult.Succeeded = true;
            return _taskResult;

        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, _dbSet.ToQueryString());
            _taskResult.AddError("در فرآیند حذف کردن این موجودیت خطایی رخ داد");
            _taskResult.AddError("جهت مشاهده اطلاعات کامل تر به بخش گزارشات مراجعه کنید");
            return _taskResult;
        }
    }
    public async Task<TaskResult> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            _taskResult.Succeeded = true;
            return _taskResult;

        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, _dbSet.ToQueryString());
            _taskResult.AddError("در فرآیند به روزرسانی کردن این موجودیت خطایی رخ داد");
            _taskResult.AddError("جهت مشاهده اطلاعات کامل تر به بخش گزارشات مراجعه کنید");
            return _taskResult;
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        if (filter is null) throw new Exception("Filter cannot be null");
        IQueryable<T> query = _dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return await query.AnyAsync(filter);
    }
    public async Task<TaskResult> SaveChangesAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
            _taskResult.Succeeded = true;
            return _taskResult;
        }
        catch (Exception ex)
        {
            _taskResult.AddError(ex.ToString());
            _myLogger.Fatal(ex, "SaveChanges");
            return _taskResult;
        }
    }
}