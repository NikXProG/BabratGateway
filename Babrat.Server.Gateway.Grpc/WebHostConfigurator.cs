using System.Net;
using Babrat.Server.Core;
using Babrat.Server.Gateway.Grpc.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace Babrat.Server.Gateway.Grpc;

/// <summary>
/// 
/// </summary>
public sealed class WebHostConfigurator:
    IWebHostConfigurator
{
    
    #region RGU.WebProgramming.Server.Core.IWebHostConfigurator implementation
    
    /// <inheritdoc cref="IWebHostConfigurator.Configure" />
    public void Configure(
        IWebHostBuilder webHostBuilder)
    {
        webHostBuilder
            .ConfigureKestrel((context, options) =>
            {
                var serverSettings = new ServerSettings();
                context.Configuration.Bind(nameof(ServerSettings), serverSettings);
              
                options.Listen(
                    IPAddress.Parse(serverSettings.ListenAddress),
                    serverSettings.ListenPort,
                    listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                        
                        // listenOptions.UseHttps(serverSettings.CertPath, serverSettings.CertPassword);
                    });
            });
    }
    
    #endregion
    
}