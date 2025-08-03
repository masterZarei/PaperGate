using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Entities.Template;
using PaperGate.Core.Interfaces;

namespace PaperGate.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<PostInfo> Posts { get; set; }
        public DbSet<CategoryInfo> Categories { get; set; }
        public DbSet<KeywordInfo> Keywords { get; set; }
        public DbSet<PostKeywordInfo> PaperKeywords { get; set; }
        public DbSet<MessageInfo> Messages { get; set; }
        public DbSet<AboutUsInfo> AboutUs { get; set; }
        public DbSet<ContactWayInfo> ContactWays { get; set; }





        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SaveConfig();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            SaveConfig();
            return base.SaveChanges();
        }
        internal async void SaveConfig()
        {
            var now = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = now;
                        entry.Entity.ModifiedOn = now;
                        break;

                    case EntityState.Modified:
                        Entry(entry.Entity).Property(x => x.CreatedOn).IsModified = false;
                        entry.Entity.ModifiedOn = now;
                        break;
                    case EntityState.Deleted:
                        break;
                }
            }
        }
    }

}
