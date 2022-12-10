using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public interface IExternalEventsService : IDisposable
    {
        Task<TResponse> Execute<TRequest, TResponse>(string name, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IMessage, new()
            where TResponse : IMessage, new();
    }
}