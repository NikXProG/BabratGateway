using System.Text;
using System.Text.Json;
using Babrat.Server.Core;
using DryIoc.Messages;

namespace Babrat.Server.Gateway.MessageBus.Producer.Serializers;

public class JsonMessageSerializer : IMessageSerializer
{
    
    public byte[] Serialize<T>(T obj) =>
        Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
    
    public T? Deserialize<T>(byte[] obj) =>
        JsonSerializer.Deserialize<T>(obj);
    
    public string GetContentType()
    {
        return "application/json";
    }
    
}