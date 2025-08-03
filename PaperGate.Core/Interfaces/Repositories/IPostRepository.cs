using PaperGate.Core.Entities;

namespace PaperGate.Core.Interfaces.Repositories;
public interface IPostRepository : IGenericRepository<PostInfo>
{
    Task<PostInfo>? GetPostBySlugAsync(string? slug);
}