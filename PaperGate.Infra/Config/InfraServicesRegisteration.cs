using Microsoft.Extensions.DependencyInjection;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Infra.Implementations;
using PaperGate.Infra.Implementations.Repositories;
using PaperGate.Infra.Implementations.Service;

namespace PaperGate.Infra.Config;
public static class InfraServicesRegisteration
{
    public static void RegisterServices(this IServiceCollection service)
    {
        service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        // service.AddScoped<IPublicInfoService, PublicInfoService>();
        // service.AddScoped<IBlogRepository, BlogRepository>();
        // service.AddScoped<IHTMLToolsService, HTMLToolsService>();

    }

}
