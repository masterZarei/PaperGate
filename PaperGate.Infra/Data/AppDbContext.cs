using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;

namespace PaperGate.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<PaperInfo> Papers { get; set; }
        public DbSet<CommentInfo> Comments { get; set; }

    }
}
