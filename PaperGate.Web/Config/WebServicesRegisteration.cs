using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;

namespace PaperGate.Web.Config;

public static class WebServicesRegisteration
{
    public static void RegisterServices(this IServiceCollection service)
    {
        //  service.AddScoped<IFileManagementService, FileManagementService>();
    }
}
