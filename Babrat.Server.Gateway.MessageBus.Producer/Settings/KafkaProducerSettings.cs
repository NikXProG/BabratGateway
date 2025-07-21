using System.Net;

namespace Babrat.Server.Gateway.MessageBus.Producer.Settings;

public class KafkaProducerSettings
{
    
    #region Fields

    private string _targetAddress;

    #endregion
    
    #region Properties
    
    public string TargetAddress
    {
        get =>
            _targetAddress.Equals("localhost")
                ? IPAddress.Loopback.ToString()
                : _targetAddress;

        set =>
            _targetAddress = value;
        
    }
    
    
    public ushort TargetPort
    {
        get;

        set;
    }

    public string Acks
    {
        get; 
        set;
    }
    
    public ushort MessageTimeoutMs
    {
        get;

        set;
    }
    
    
    #endregion
    
    
    
}