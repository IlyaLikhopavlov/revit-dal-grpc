using App.CommunicationServices.Grpc;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DML;
using AutoMapper;
using Revit.Services.Grpc.Services;

namespace App.DAL.Common.Services.RevitEntities;

public class LevelsRoomsService
{
    private readonly DocumentDescriptor _documentDescriptor;
    private readonly RevitExtraDataExchangeClient _client;

    public LevelsRoomsService(
        RevitExtraDataExchangeClient client,
        DocumentDescriptor documentDescriptor)
    {
        _client = client;
        _documentDescriptor = documentDescriptor;
    }

    private async Task<Level[]> GetRevitGrpcLevelsAsync()
    {
        var levels = await _client.GetLevelsFromRevit(_documentDescriptor);

        return levels;
    }

    private List<BcLevel> MapGrpcLevelsToBcLevels(Level[] levels)
    {
        var config = new MapperConfiguration(mc => mc.CreateMap<Level, BcLevel>()
            .ForMember(val => val.Description, act => act.MapFrom(src => src.Value))
            .ForMember(rooms => rooms.Rooms, act => act.MapFrom(src => src.Rooms)));

        var mapper = new Mapper(config);

        var bcLevels = mapper.Map<List<Level>,List<BcLevel>>(levels.ToList());

        return bcLevels;
    }

    public async Task<List<BcLevel>> GetRevitLevelsAsync()
    {
        var grpcLevels = await GetRevitGrpcLevelsAsync();

        var bcLevels = MapGrpcLevelsToBcLevels(grpcLevels);

        return bcLevels;
    }
}