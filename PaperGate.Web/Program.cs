using Microsoft.AspNetCore.Authentication.Cookies;
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
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Globalization;

namespace PaperGate.Web;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Logging.AddDebug();

        #region Serilog

        var logger = new LoggerConfiguration()
                        // add console as logging target
                        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                        // add a logging target for warnings and higher severity  logs
                        // structured in JSON format
                        .WriteTo.File(new JsonFormatter(),
                                      "important.json",
                                      restrictedToMinimumLevel: LogEventLevel.Warning)
                        // add a rolling file for all logs
                        .WriteTo.File("all-.logs",
                                      rollingInterval: RollingInterval.Day)
                        // set default minimum level
                        .MinimumLevel.Debug()
                        .CreateLogger();
        builder.Host.UseSerilog((context, configuration) =>
             configuration.ReadFrom.Configuration(context.Configuration));

        builder.Logging.AddConsole();
        #endregion
        #region Services
        RegisterServices(builder.Services);
        #region Context
        var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        //builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        #endregion


        #region Localization
        // اضافه کردن Localization
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        // تنظیم زبان‌های پشتیبانی‌شده
        var supportedCultures = new[]
        {
          new CultureInfo("fa"),
          new CultureInfo("en")
        };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("fa"); // پیش‌فرض فارسی
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        #endregion


        #region Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = StaticValues.LoginPath;
            options.LogoutPath = StaticValues.LogoutPath;
            options.AccessDeniedPath = StaticValues.AccessDeniedPath;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
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
            options.JsonSerializerOptions.WriteIndented = true; // Optional for readability
            options.JsonSerializerOptions.MaxDepth = 128; // افزایش عمق به مقدار مورد نیاز

        }).AddViewLocalization();
        builder.Services.AddAntiforgery();
        #endregion

        builder.Services.AddHealthChecks()
            .AddCheck("Self", () => HealthCheckResult.Healthy("Application is running"));

        #endregion

        #region Identity Config
        builder.Services.AddIdentity<UserInfo, IdentityRole>(r =>
        {
            r.Tokens = new TokenOptions()
            {
                AuthenticatorIssuer = "Emzacode.com"
            };
            r.Password = new PasswordOptions()
            {
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false,
                RequiredLength = 3,
                RequiredUniqueChars = 0,
            };
        })
              .AddRoles<IdentityRole>()
              .AddErrorDescriber<IdentityErrorHandler>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddTokenProvider<DataProtectorTokenProvider<UserInfo>>(TokenOptions.DefaultProvider);
        #endregion

        builder.Services.AddControllers();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        // Seed admin
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            IdentitySeed.SeedAdmin(services).GetAwaiter().GetResult();
        }
        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseStaticFiles();

        app.UseSerilogRequestLogging();
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAntiforgery();
        app.UseAuthorization();


        app.MapRazorPages();
        app.MapControllers();
        #region Health check endpoint
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
        #endregion
        app.UseHealthChecks("/health");
        #region NotFound
        /*app.UseExceptionHandler("/NotFound");
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.Redirect("/NotFound");
            }
        });*/

        #endregion

        #region NotFound
        app.UseExceptionHandler("/NotFound");
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.Redirect("/NotFound");
            }
        });
        #endregion
        app.Run();
    }
    public static void RegisterServices(IServiceCollection services)
    {
        InfraServicesRegisteration.RegisterServices(services);
        WebServicesRegisteration.RegisterServices(services);
    }
}
