using App.CommunicationServices.Grpc;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DML;
using AutoMapper;
using Revit.Services.Grpc.Services;
using Room = Revit.Services.Grpc.Services.Room;
using BuildingRoom = App.DML.Room;

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

    private List<BuildingLevel> MapGrpcLevelsToBcLevels(Level[] levels)
    {
        var config = new MapperConfiguration(mc =>
        {
            mc.CreateMap<Level, BuildingLevel>()
                .ForMember(val => val.Elevation, act => act.MapFrom(src => src.Value));
            mc.CreateMap<Room,BuildingRoom>()
                .ForMember(r=> r.Number, act => act.MapFrom(src => src.Number));
        });
            // .ForMember(rooms => rooms.Rooms, act => act.MapFrom(src => src.Rooms)));
        
        
        var mapper = new Mapper(config);

        var bcLevels = mapper.Map<List<Level>,List<BuildingLevel>>(levels.ToList());

        return bcLevels;
    }

    public async Task<List<BuildingLevel>> GetRevitLevelsAsync()
    {
        var grpcLevels = await GetRevitGrpcLevelsAsync();

        var bcLevels = MapGrpcLevelsToBcLevels(grpcLevels);

        return bcLevels;
    }
}