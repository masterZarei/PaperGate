using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Shared.ReturnTypes;

namespace PaperGate.Core.Interfaces;
public interface IUnitOfWork
{
    IGenericRepository<PaperInfo> Paper { get; }
    IGenericRepository<CategoryInfo> Category { get; }
    IGenericRepository<PaperCategoryInfo> PaperCategory { get; }
    IGenericRepository<KeywordInfo> Keyword { get; }
    IGenericRepository<PaperKeywordInfo> PaperKeyword { get; }

    Task<TaskResult> SaveChangesAsync();
}
