using System.Collections.Concurrent;
using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public class RequestsQueue : IRequestsQueue
    {
        private readonly BlockingCollection<RequestObject> _jobs = new();

        public RequestsQueue()
        {
            Task.Run(OnStart);
        }

        public async Task<IMessage> Enqueue(RequestObject request)
        {
            _jobs.Add(request);
            return await request.GetResponse();
        }

        public Func<RequestObject, Task<IMessage>> RequestProcessor { get; set; }

        private async void OnStart()
        {
            foreach (var requestObject in _jobs.GetConsumingEnumerable(CancellationToken.None))
            {
                await RequestProcessor?.Invoke(requestObject)!;
            }
        }
    }
}
