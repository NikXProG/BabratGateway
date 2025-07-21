using Babrat.Server.Core;
using Babrat.Server.Core.Exceptions;
using Babrat.Server.Gateway.MessageBus.Consumer.Serializers;
using Babrat.Server.Gateway.MessageBus.Consumer.Settings;
using Confluent.Kafka;
using DryIoc;
using Microsoft.Extensions.Configuration;

namespace Babrat.Server.Gateway.MessageBus.Consumer;

public sealed class ServiceRegistrator:
    IServiceRegistrator
{
    
    #region RGU.WebProgramming.Server.Core.IServiceRegistrator implementation
    
    /// <inheritdoc cref="IServiceRegistrator.Register" />
    public void Register(
        IRegistrator registrator,
        IConfiguration configuration)
    {
        
        registrator.Configure<KafkaConsumerSettings>(
            configuration.GetSection($"Kafka:{nameof(KafkaConsumerSettings)}"));
        registrator.Register<IMessageBusReceiver, MessageBusReceiver>(Reuse.Singleton);
        registrator.Register<IMessageSerializer, JsonMessageSerializer>(Reuse.Singleton);
        
    }
    
    #endregion
    
}