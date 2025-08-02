using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Entities.Template;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Shared.ReturnTypes;

namespace PaperGate.Core.Interfaces;
public interface IUnitOfWork
{
    IGenericRepository<PostInfo> Post { get; }
    IGenericRepository<CategoryInfo> Category { get; }
    IGenericRepository<KeywordInfo> Keyword { get; }
    IGenericRepository<PaperKeywordInfo> PaperKeyword { get; }
    IGenericRepository<MessageInfo> Message { get; }
    IGenericRepository<AboutUsInfo> AboutUs { get; }

    Task<TaskResult> SaveChangesAsync();
}
