using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Config;
using PaperGate.Infra.Config;
using PaperGate.Web.Config;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using PaperGate.Core.Libraries.StaticValues;
using Microsoft.AspNetCore.Authentication.Cookies;
using PaperGate.Web.Utilities.Libraries;

namespace PaperGate.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
        builder.Services.AddInfraDbContext(connectionString!);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        RegisterServices(builder.Services, args);


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
        .AddPolicy(Roles.AdminEndUser, p => p.RequireRole(Roles.AdminEndUser))
        .AddPolicy(Roles.StudentEndUser, p => p.RequireAuthenticatedUser());


        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/Account", Roles.StudentEndUser);
            options.Conventions.AuthorizeFolder("/Account/Admin", Roles.AdminEndUser);

        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.WriteIndented = true; // Optional for readability
            options.JsonSerializerOptions.MaxDepth = 128; // افزایش عمق به مقدار مورد نیاز

        });
        #endregion
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

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
    public static void RegisterServices(IServiceCollection services, string[] args)
    {
        WebServicesRegisteration.RegisterServices(services);
        InfraServicesRegisteration.RegisterServices(services);
        CoreServicesRegisteration.RegisterServices(services);
    }
}
