using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Entities.Template;
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

    private IPostRepository _post;
    private IGenericRepository<CategoryInfo> _category;
    private IGenericRepository<KeywordInfo> _keyword;
    private IGenericRepository<PostKeywordInfo> _paperKeyword;
    private IGenericRepository<MessageInfo> _message;
    private IGenericRepository<AboutUsInfo> _aboutUs;
    private IGenericRepository<ContactWayInfo> _contactWay;

    public UnitOfWork(AppDbContext db, ILogger myLogger)
    {
        _db = db;
        _myLogger = myLogger;
        _taskResult = new();
    }

    public IPostRepository Post => _post ??= new PostRepository(_db, _myLogger);
    public IGenericRepository<CategoryInfo> Category => _category ??= new GenericRepository<CategoryInfo>(_db, _myLogger);
    public IGenericRepository<KeywordInfo> Keyword => _keyword ??= new GenericRepository<KeywordInfo>(_db, _myLogger);
    public IGenericRepository<PostKeywordInfo> PaperKeyword => _paperKeyword ??= new GenericRepository<PostKeywordInfo>(_db, _myLogger);
    public IGenericRepository<MessageInfo> Message => _message ??= new GenericRepository<MessageInfo>(_db, _myLogger);
    public IGenericRepository<AboutUsInfo> AboutUs => _aboutUs ??= new GenericRepository<AboutUsInfo>(_db, _myLogger);
    public IGenericRepository<ContactWayInfo> ContactWay => _contactWay ??= new GenericRepository<ContactWayInfo>(_db, _myLogger);

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

/*public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly ILogger _myLogger;
    protected TaskResult _taskResult;

    public UnitOfWork(AppDbContext db, ILogger myLogger)
    {
        _db = db;
        _myLogger = myLogger;
        _taskResult = new();

        Post = new PostRepository(_db, myLogger);
        Category = new GenericRepository<CategoryInfo>(_db, myLogger);
        Keyword = new GenericRepository<KeywordInfo>(_db, myLogger);
        PaperKeyword = new GenericRepository<PostKeywordInfo>(_db, myLogger);
        Message = new GenericRepository<MessageInfo>(_db, myLogger);
        AboutUs = new GenericRepository<AboutUsInfo>(_db, myLogger);
        ContactWay = new GenericRepository<ContactWayInfo>(_db, myLogger);
    }
    public IGenericRepository<CategoryInfo> Category { get; set; }
    public IGenericRepository<KeywordInfo> Keyword { get; set; }
    public IGenericRepository<PostKeywordInfo> PaperKeyword { get; set; }
    public IGenericRepository<MessageInfo> Message { get; set; }
    public IGenericRepository<AboutUsInfo> AboutUs { get; set; }
    public IGenericRepository<ContactWayInfo> ContactWay { get; set; }
    public IPostRepository Post { get; set; }

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
*/