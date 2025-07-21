using Confluent.Kafka;

namespace Babrat.Server.Core;

public interface IMessageBusReceiver
{

     Task<string> ReceiveAsync<TResponse>(
        Guid requestId,
        string topic,
        CancellationToken cancellationToken = default);
    

}