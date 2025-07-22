using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Infra.Data;
using PaperGate.Infra.Implementations.Repositories;
using PaperGate.Shared.ReturnTypes;
using Serilog;

namespace PaperGate.Infra.Implementations;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly ILogger _myLogger;
    protected TaskResult _taskResult;

    public UnitOfWork(AppDbContext db, ILogger myLogger)
    {
        _db = db;
        _myLogger = myLogger;
        _taskResult = new();

        Paper = new GenericRepository<PostInfo>(_db, myLogger);
        Category = new GenericRepository<CategoryInfo>(_db, myLogger);
        Keyword = new GenericRepository<KeywordInfo>(_db, myLogger);
        PaperKeyword = new GenericRepository<PaperKeywordInfo>(_db, myLogger);
    }
    public IGenericRepository<PostInfo> Paper { get; set; }
    public IGenericRepository<CategoryInfo> Category { get; set; }
    public IGenericRepository<KeywordInfo> Keyword { get; set; }
    public IGenericRepository<PaperKeywordInfo> PaperKeyword { get; set; }

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
            _myLogger.Fatal(ex, "UnitOfWork-SaveChanges", LoggingLevel.Fatal);
            return _taskResult;
        }
    }
}
