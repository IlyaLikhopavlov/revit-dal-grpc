using App.CommunicationServices.ScopedServicesFunctionality;
using App.DML;

namespace App.DAL.Common.Services.RevitEntities;

public class RevitEntitiesService
{
    private readonly LevelsRoomsService _levelsRoomsService;
    
    public RevitEntitiesService(IDocumentDescriptorServiceScopeFactory scopeFactory)
    {
            _levelsRoomsService = scopeFactory.GetScopedService<LevelsRoomsService>();
    }

    public async Task<List<BcLevel>> GetRevitLevelsAsync()
    {
        return await _levelsRoomsService.GetRevitLevelsAsync();
    }
}