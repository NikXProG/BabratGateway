using Babrat.Server.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Babrat.Server.Gateway.GrpcImpl;

/// <summary>
/// 
/// </summary>
public sealed class EndpointsConfigurator:
    IEndpointsConfigurator
{
    
    #region Terminal.Host.Grpc.IEndpointsConfigurator implementation

    /// <inheritdoc cref="IEndpointsConfigurator.Configure(IEndpointRouteBuilder)"/>
    public void Configure(
        IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<Services.CreateTableService>();
        routeBuilder.MapGrpcService<Services.InsertService>();
    }

    #endregion
    
}