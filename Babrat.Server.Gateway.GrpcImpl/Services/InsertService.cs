using Babrat.Domain.Models;
using Babrat.Server.Core;
using Babrat.Server.Gateway.GrpcImpl.Settings;
using Babrat.Server.Grpc.Models;
using Grpc.Core;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InsertModel = Babrat.Domain.Models.InsertModel;
using CreateTableResponse = Babrat.Server.Grpc.Models.CreateTableResponse;

namespace Babrat.Server.Gateway.GrpcImpl.Services;

public class InsertService :
    Babrat.Server.Grpc.Models.InsertService.InsertServiceBase
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    
    private readonly IMapper  _mapper;
   
    private readonly IMessageBusSender _producer;
       
    private readonly IMessageBusReceiver _consumer;
    
    private readonly IOptions<CreateTableServiceSettings> _options;
    
    private readonly ILogger<CreateTableService>? _logger;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="consumer"></param>
    /// <param name="mapper"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="producer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public InsertService(
        IMessageBusSender producer,
        IMessageBusReceiver consumer,
        IMapper mapper,
        IOptions<CreateTableServiceSettings> options,
        ILogger<CreateTableService>? logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    #endregion
    
    #region RGU.WebProgramming.Grpc.MyFirstService.MyFirstServiceBase overrides
    
    /// <inheritdoc cref="Grpc.Models.CreateTableService.CreateTableServiceBase.CreateTable" />
    
    public override async Task<Babrat.Server.Grpc.Models.InsertModel> InsertInto(
        Babrat.Server.Grpc.Models.InsertModel request,
        ServerCallContext context)
    {
        _logger?.LogDebug($"{nameof(InsertInto)} request execution started");
        
        try
        {
            
            // foreach (var column in request.Rows)
            // {
            //     foreach (var columnValue in column.Values)
            //     {
            //         Console.WriteLine(columnValue);
            //     }
            // }
            
            
            var message = new KafkaMessage<InsertModel>(
                TypeModel.INSERT,
                _mapper.Map<InsertModel>(request)
            );
            
            // Console.WriteLine(message.RequestId);
            
            await _producer.SendAsync(
                "table-topic",
                message, 
                context.CancellationToken);
            
            // var response =  await _consumer.ReceiveAsync<string>(
            //     message.RequestId, 
            //     "db-responses", 
            //     context.CancellationToken);
            //
            // Console.WriteLine("RESPONSE" + response);
           
            _logger?.LogDebug($"{nameof(InsertInto)} request execution succeeded.");
            
            return await Task.FromResult(
                _mapper.Map<Babrat.Server.Grpc.Models.InsertModel>(
                    _mapper.Map<InsertModel>(request))
            );
            
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Failed to execute {nameof(InsertInto)} request");
            
            return await Task.FromResult(new Grpc.Models.InsertModel());
        }
    }

    #endregion
}