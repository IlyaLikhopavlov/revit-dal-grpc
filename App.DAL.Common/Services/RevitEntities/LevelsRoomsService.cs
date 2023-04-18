using App.CommunicationServices.Grpc;

namespace App.DAL.Common.Services.RevitEntities;

public class LevelsRoomsService
{
    public DocumentDescriptor DocumentDescriptor { get; }

    public LevelsRoomsService(
        DocumentDescriptor documentDescriptor)
    {
        DocumentDescriptor = documentDescriptor;
    }
    
}