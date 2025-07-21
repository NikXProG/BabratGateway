namespace Babrat.Domain.Models;

public enum TypeModel{
    //ddl
    CREATE_TABLE,
    CREATE_NAMESPACE,
    CREATE_DATABASE,
    DROP_DATABASE,
    DROP_NAMESPACE,
    DROP_TABLE,
    //dml
    INSERT,
    DELETE,
    UPDATE,
    SELECT
};



public record KafkaMessage<T>
{
    private readonly Guid _requestId;
    
    private readonly String _typeModel;
    
    private readonly T _payload;

    public KafkaMessage(TypeModel model, T payload)
    {
        _requestId = Guid.NewGuid();
        _typeModel = model.ToString();
        _payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }
    
    public Guid RequestId => _requestId;
    
    public String TypeModel => _typeModel;
    public T Payload => _payload;
    
    
}