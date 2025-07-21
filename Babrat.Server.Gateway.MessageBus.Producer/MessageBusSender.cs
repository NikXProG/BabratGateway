using System.Text;
using System.Text.Json;
using Babrat.Server.Core;
using Babrat.Server.Gateway.MessageBus.Producer.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Babrat.Server.Gateway.MessageBus.Producer;


public sealed class MessageBusSender : IMessageBusSender, IDisposable
{

    private readonly IProducer<Null, byte[]> _producer;
        
    private readonly IMessageSerializer _serializer;
    
    public MessageBusSender(
        IMessageSerializer serializer, 
        IOptions<KafkaProducerSettings> options)
    {
         if (options?.Value == null)
            throw new ArgumentException("Invalid KafkaProducerSettings: BootstrapServers is required.");
        
         var kafkaSettings = options.Value;
         
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        
        _producer = new ProducerBuilder<Null, byte[]>(
                new ProducerConfig
            {
                BootstrapServers = $"{kafkaSettings.TargetAddress}:{kafkaSettings.TargetPort}",
                Acks = Enum.Parse<Acks>(kafkaSettings.Acks),
                MessageTimeoutMs = kafkaSettings.MessageTimeoutMs
            })
            .Build();
    }
    
    public async Task<DeliveryResult<Null, byte[]>> SendAsync<T>(
        string topic,
        T message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(topic);
        ArgumentNullException.ThrowIfNull(message);
        
        Console.WriteLine($"Sending message of type {typeof(T).Name} to topic {topic}");
        
        try
        {
            return await _producer.ProduceAsync(
                topic,
                new Message<Null, byte[]>
                {
                    Value =  _serializer.Serialize(message),
                    Headers = new Headers
                    {
                        { "Content-Type", 
                            Encoding.UTF8.GetBytes( _serializer.GetContentType()) }
                    }
                },
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (ProduceException<Null, byte[]> ex)
        {
            throw new InvalidOperationException($"Failed to deliver message to topic {topic}", ex);
        }
    }
    
    public void Dispose()
    {
        _producer?.Dispose();
     
    }
    
}


