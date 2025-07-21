using Babrat.Server.Core;
using Babrat.Server.Core.Exceptions;
using Babrat.Server.Gateway.MessageBus.Producer;
using Babrat.Server.Gateway.MessageBus.Producer.Serializers;
using Babrat.Server.Gateway.MessageBus.Producer.Settings;
using Confluent.Kafka;
using DryIoc;
using Microsoft.Extensions.Configuration;

namespace Babrat.Server.MessageBus.Producer;

public sealed class ServiceRegistrator:
    IServiceRegistrator
{
    
    #region RGU.WebProgramming.Server.Core.IServiceRegistrator implementation
    
    /// <inheritdoc cref="IServiceRegistrator.Register" />
    public void Register(
        IRegistrator registrator,
        IConfiguration configuration)
    {
        
        registrator.Configure<KafkaProducerSettings>(
            configuration.GetSection($"Kafka:{nameof(KafkaProducerSettings)}"));
        registrator.Register<IMessageBusSender, MessageBusSender>(Reuse.Singleton);
        registrator.Register<IMessageSerializer, JsonMessageSerializer>(Reuse.Singleton);
    }
    
    #endregion
    
}