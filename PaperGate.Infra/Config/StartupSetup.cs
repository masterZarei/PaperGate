using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;

namespace PaperGate.Infra.Config;
public static class StartupSetup
{
    public static void AddInfraDbContext(this IServiceCollection services, string connectionString)
    {

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        //services.AddDatabaseDeveloperPageExceptionFilter();

        #region Identity
        services.AddIdentity<UserInfo, IdentityRole>(r =>
        {
            r.Tokens = new TokenOptions()
            {
                AuthenticatorIssuer = "PaperGate"
            };
            r.Password = new PasswordOptions()
            {
#if DEBUG
                RequireDigit = true,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false,
                RequiredLength = 3,
                RequiredUniqueChars = 0,
#endif

            };
        })
              .AddRoles<IdentityRole>()
              .AddErrorDescriber<IdentityErrorHandler>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
        #endregion
    }
}
