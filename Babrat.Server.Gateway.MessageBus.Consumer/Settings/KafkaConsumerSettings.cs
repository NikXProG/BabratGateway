using System.Net;

namespace Babrat.Server.Gateway.MessageBus.Consumer.Settings;

public class KafkaConsumerSettings
{
    #region Fields

    private string _listenAddress;

    #endregion
    
    #region Properties
    
    public string ListenAddress
    {
        get =>
            _listenAddress.Equals("localhost")
                ? IPAddress.Loopback.ToString()
                : _listenAddress;

        set =>
            _listenAddress = value;
        
    }
    
    
    public ushort ListenPort
    {
        get;

        set;
    }
    
    public string GroupId
    {
        get;

        set;
    }

    public string AutoOffsetReset
    {
        get;

        set;
    }
    
    
    #endregion
}