using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Config;
using PaperGate.Infra.Config;
using PaperGate.Web.Config;

namespace PaperGate.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddInfraDbContext(connectionString!);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        RegisterServices(builder.Services);
        builder.Services.AddRazorPages();

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
    public static void RegisterServices(IServiceCollection services)
    {
        CoreServicesRegisteration.RegisterServices(services);
        InfraServicesRegisteration.RegisterServices(services);
        WebServicesRegisteration.RegisterServices(services);
    }
}
