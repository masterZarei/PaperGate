using PaperGate.Core.Config;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Services;

namespace PaperGate.Web.Config;

public static class WebServicesRegisteration
{
    public static void RegisterServices(this IServiceCollection service)
    {
          service.AddScoped<IFileManagementService, FileManagementService>();
        service.AddAutoMapper(typeof(MappingProfile).Assembly);

    }
}
