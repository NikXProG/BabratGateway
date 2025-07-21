using Google.Protobuf.Collections;
using Mapster;

namespace Babrat.Server.Gateway.GrpcImpl.Mapping;

public class TableMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default
            .UseDestinationValue(member => member.SetterModifier == AccessModifier.None &&
                                           member.Type.IsGenericType &&
                                           member.Type.GetGenericTypeDefinition() == typeof(RepeatedField<>));
        
        config.NewConfig<
                Babrat.Domain.Models.CreateTableModel,
                Babrat.Server.Grpc.Models.CreateTableModel>()
            .Map(dest => dest.TableName, src => src.TableName)
            .RequireDestinationMemberSource(true);
       
        config.NewConfig<
                Babrat.Domain.Models.ColumnModel,
                Babrat.Server.Grpc.Models.ColumnModel>()
            .Map(dest => dest.Default, src => src.Default ?? string.Empty) 
            .RequireDestinationMemberSource(true);

        config.NewConfig<
            Babrat.Domain.Models.InsertModel,
            Babrat.Server.Grpc.Models.InsertModel>();

        config.NewConfig<Domain.Models.InsertRow,
                Grpc.Models.InsertRow>()
            .Map(
                dest => dest.Values, 
                src => src.Values
                    .Select(v => v ?? ""))
            .RequireDestinationMemberSource(true);

    }
}