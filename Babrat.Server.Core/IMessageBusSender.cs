using Confluent.Kafka;

namespace Babrat.Server.Core;

public interface IMessageBusSender
{
    public Task<DeliveryResult<Null, byte[]>> SendAsync<T>(
        string topic,
        T message,
        CancellationToken cancellationToken = default);
}