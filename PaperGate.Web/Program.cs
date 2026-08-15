using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PaperGate.Core.Entities;
using PaperGate.Core.Libraries.StaticValues;
using PaperGate.Infra.Config;
using PaperGate.Infra.Data;
using PaperGate.Web.Config;
using PaperGate.Web.Utilities.Libraries;
using Serilog;
using System.Globalization;

namespace PaperGate.Web;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) =>
             configuration.ReadFrom.Configuration(context.Configuration));

        #region Services
        RegisterServices(builder.Services);

        var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        #endregion

        #region Localization
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        var supportedCultures = new[]
        {
          new CultureInfo("fa"),
          new CultureInfo("en")
        };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("fa");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        #endregion

        #region Identity
        builder.Services.AddIdentity<UserInfo, IdentityRole>(options =>
        {
            options.Tokens.AuthenticatorIssuer = "Emzacode.com";

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.AllowedForNewUsers = true;
        })
              .AddRoles<IdentityRole>()
              .AddErrorDescriber<IdentityErrorHandler>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddTokenProvider<DataProtectorTokenProvider<UserInfo>>(TokenOptions.DefaultProvider);

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = StaticValues.LoginPath;
            options.LogoutPath = StaticValues.LogoutPath;
            options.AccessDeniedPath = StaticValues.AccessDeniedPath;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        });
        #endregion

        #region Authorization
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(Roles.AdminEndUser, p => p.RequireRole(Roles.AdminEndUser));

        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/Account/Admin", Roles.AdminEndUser);
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.MaxDepth = 128;
        }).AddViewLocalization();

        builder.Services.AddAntiforgery();
        #endregion

        builder.Services.AddControllers();
        builder.Services.AddHealthChecks()
            .AddCheck("Self", () => HealthCheckResult.Healthy("Application is running"));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/NotFound");
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            IdentitySeed.SeedAdmin(scope.ServiceProvider).GetAwaiter().GetResult();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSerilogRequestLogging();
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapRazorPages();
        app.MapControllers();

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/health")
            {
                if (!context.User.IsInRole(Roles.AdminEndUser))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access Denied");
                    return;
                }
            }
            await next();
        });
        app.UseHealthChecks("/health");

        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.Redirect("/NotFound");
            }
        });

        app.Run();
    }

    public static void RegisterServices(IServiceCollection services)
    {
        InfraServicesRegisteration.RegisterServices(services);
        WebServicesRegisteration.RegisterServices(services);
    }
}
