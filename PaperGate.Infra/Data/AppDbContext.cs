using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;

namespace PaperGate.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<PaperInfo> Papers { get; set; }
        public DbSet<CategoryInfo> Categories { get; set; }
        public DbSet<PaperCategoryInfo> PaperCategories { get; set; }
        public DbSet<KeywordInfo> Keywords { get; set; }
        public DbSet<PaperKeywordInfo> PaperKeywords { get; set; }

    }
}
