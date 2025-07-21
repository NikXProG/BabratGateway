using System.Reflection;
using Babrat.Server.Core;
using Babrat.Server.Core.Exceptions;
using Babrat.Server.Gateway.GrpcImpl.Services;
using Babrat.Server.Gateway.GrpcImpl.Settings;
using DryIoc;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;

namespace Babrat.Server.Gateway.GrpcImpl;

/// <summary>
/// 
/// </summary>
public class ServiceRegistrator:
    IServiceRegistrator
{
    
    #region Babrat.Server.Core.IServiceRegistrator implementation
    
    /// <inheritdoc cref="IServiceRegistrator.Register" />
    public void Register(
        IRegistrator registrator,
        IConfiguration configuration)
    {
        registrator
            .Configure<CreateTableServiceSettings>(
                configuration.GetSection(nameof(CreateTableServiceSettings)))
            .Register<IEndpointsConfigurator, EndpointsConfigurator>();
        
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            registrator.RegisterInstance(config);
            
    }
    
    #endregion
    
}