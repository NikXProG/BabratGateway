using Babrat.Server.Core;
using Babrat.Server.Core.Exceptions;
using Babrat.Server.Gateway.Grpc.Settings;
using DryIoc;
using Microsoft.Extensions.Configuration;

namespace Babrat.Server.Gateway.Grpc;

public class ServiceRegistrator
    :  IServiceRegistrator
{
    
    
    #region Babrat.Server.Core.IServiceRegistrator implementation
    
    public void Register(
        IRegistrator registrator,
        IConfiguration configuration)
    {
        registrator
            .Configure<ServerSettings>(configuration.GetSection(nameof(ServerSettings)))
            .RegisterMany<ApplicationConfigurator>();
        
    }
    
    #endregion
    
    
}