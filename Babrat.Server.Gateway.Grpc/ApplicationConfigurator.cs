using Babrat.Server.Core;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Babrat.Server.Gateway.Grpc;

public sealed class ApplicationConfigurator:
    IApplicationConfigurator,
    IStartup
{
    
    #region  Babrat.Server.Core.IApplicationConfigurator implementation

    /// <inheritdoc cref="IApplicationConfigurator.Configure" />
    public void Configure(
        IApplicationBuilder applicationBuilder)
    {
        var endpointsConfigurators = applicationBuilder.ApplicationServices.GetServices<IEndpointsConfigurator>();

        applicationBuilder
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(routeBuilder =>
            {
                foreach (var endpointsConfigurator in endpointsConfigurators)
                {
                    endpointsConfigurator.Configure(routeBuilder);
                }
            });
    }
    
    #endregion
    
    #region  Babrat.Server.Core.IStartup implementation

    /// <inheritdoc cref="Core.IStartup.ConfigureServices(HostBuilderContext, IServiceCollection)" />
    public void ConfigureServices(
        HostBuilderContext ctx,
        IServiceCollection services)
    {

        services.AddControllers();
        services.AddMapster();
        services.AddAuthentication();
        services.AddAuthorization();
        services.AddGrpc();
    }

    #endregion
    
}