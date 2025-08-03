using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Infra.Data;
using PaperGate.Shared.ReturnTypes;
using Serilog;

namespace PaperGate.Infra.Implementations.Repositories;
public class PostRepository : GenericRepository<PostInfo>, IPostRepository
{
    private readonly AppDbContext _context;
    protected TaskResult _taskResult;
    public PostRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
        _context = context;
        _taskResult = new();
    }


    public async Task<PostInfo?> GetPostBySlugAsync(string? slug)
    {

        if (slug is null)
            return null;

        return await _context.Posts
            .Include(b => b.Category)
            .Include(b => b.Keywords)
            .ThenInclude(b => b.Keyword)
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);
    }
}