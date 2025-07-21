using System.Text;
using System.Text.Json;
using Babrat.Domain.Models;
using Babrat.Server.Core;
using Babrat.Server.Gateway.MessageBus.Consumer.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Babrat.Server.Gateway.MessageBus.Consumer;


public sealed class MessageBusReceiver : IMessageBusReceiver, IDisposable
{
    private readonly IConsumer<Null, byte[]> _consumer;
    private readonly IMessageSerializer _serializer;
    private readonly ILogger<MessageBusReceiver> _logger;

    public MessageBusReceiver(
        IMessageSerializer serializer,
        IOptions<KafkaConsumerSettings> options,
        ILogger<MessageBusReceiver> logger)
    {
        var kafkaSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _logger = logger;

        _consumer = new ConsumerBuilder<Null, byte[]>(
            new ConsumerConfig
            {
                BootstrapServers = $"{kafkaSettings.ListenAddress}:{kafkaSettings.ListenPort}",
                GroupId = kafkaSettings.GroupId,
                AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaSettings.AutoOffsetReset),
            })
            .SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}"))
            .SetLogHandler((_, log) => _logger.LogInformation($"Kafka log: {log.Message}"))
            .Build();
    }

    public async Task<string> ReceiveAsync<TResponse>(
        Guid requestId,
        string topic,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(topic))
            throw new ArgumentException("Topic cannot be empty", nameof(topic));

        try
        {
            _consumer.Subscribe(topic);
            _logger.LogDebug("Subscribed to topic {Topic}", topic);
            

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                   
                    // var consumeResult = _consumer.Consume(cancellationToken);
                    // _logger.LogDebug("Received message: {Value}", consumeResult.Message.Value);
                    //
                    // var messageJson = Encoding.UTF8.GetString(consumeResult.Message.Value);
                    //
                    // return messageJson;
                    
                    // var messageJson = Encoding.UTF8.GetString(consumeResult.Message.Value);
                    // _logger.LogDebug("Raw JSON: {Json}", messageJson);
                    //
                    // var message = _serializer.Deserialize<KafkaMessage<TResponse>>(consumeResult.Message.Value);
                    //
                    //
                    // if (message.RequestId == requestId)
                    // {
                    //     _consumer.Commit(consumeResult); 
                    //     _consumer.StoreOffset(consumeResult);
                    //     return message.Payload;
                    // }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error");
                    if (ex.Error.IsFatal)
                        throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize or process message");
                }
            }
        }
        finally
        {
            _logger.LogInformation("Consumer stopped");
        }

        cancellationToken.ThrowIfCancellationRequested();
        throw new InvalidOperationException("Failed to receive message");
    }

    public void Dispose()
    {
        _consumer?.Dispose();
    }
}
