using Babrat.Server.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Babrat.Server.Gateway.MessageBus.Consumer;

public class ApplicationConfigurator : IApplicationConfigurator, IStartup
{
    #region RGU.WebProgramming.Server.Core.IApplicationConfigurator implementation
    
    /// <inheritdoc cref="IApplicationConfigurator.Configure" /> 
    public void Configure(
        IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseRouting();
        applicationBuilder.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.MapControllers();
        });
    }
    
    #endregion
    
    #region RGU.WebProgramming.Server.Core.IStartup implementation
    
    /// <inheritdoc cref="Core.IStartup.ConfigureServices" />
    public void ConfigureServices(
        HostBuilderContext ctx,
        IServiceCollection services)
    {
        services.AddControllers();
    }
      
    #endregion
    
    
}