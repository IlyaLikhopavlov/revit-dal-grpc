using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public interface IRequestsQueue
    {
        Task<IMessage> Enqueue(RequestObject request);

        Func<RequestObject, Task<IMessage>> RequestProcessor { set; }
    }
}
