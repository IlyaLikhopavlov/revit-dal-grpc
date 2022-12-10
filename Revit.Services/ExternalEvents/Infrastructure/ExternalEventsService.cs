using System.Collections.Concurrent;
using Autodesk.Revit.UI;
using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public class ExternalEventsService : IExternalEventsService
    {
        private ConcurrentDictionary<string, IExternalServiceEventHandler> _externalEventHandlersDictionary = new();

        private readonly IRequestsQueue _requestsQueue;

        private readonly IEnumerable<IExternalServiceEventHandler> _handlers;

        public ExternalEventsService(
            IRequestsQueue requestsQueue, 
            IEnumerable<IExternalServiceEventHandler> handlers)
        {
            _requestsQueue = requestsQueue;
            _handlers = handlers;
            requestsQueue.RequestProcessor = ProcessRequest;
            Register();
        }

        private async Task<IMessage> ProcessRequest(RequestObject request)
        {
            var externalEventHandler = GetExternalEventHandler(request.HandlerId);
            externalEventHandler.SetRequest(request);

            var response = await request.GetResponse();

            return response;
        }

        private void Register()
        {
            foreach (var externalEventHandler in _handlers)
            {
                if (_externalEventHandlersDictionary.ContainsKey(externalEventHandler.GetName()))
                {
                    return;
                }

                var evt = ExternalEvent.Create(externalEventHandler);
                externalEventHandler.ExternalEvent = evt;
                _externalEventHandlersDictionary.TryAdd(externalEventHandler.GetName(), externalEventHandler);
            }
        }
        
        private IExternalServiceEventHandler GetExternalEventHandler(string name)
        {
            if (!_externalEventHandlersDictionary.TryGetValue(name, out var externalEvent))
            {
                throw new ArgumentException($"Required external event handler didn't find by name={name}.");
            }

            return externalEvent;
        }

        public async Task<TResponse> Execute<TRequest, TResponse>(string name, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IMessage, new()
            where TResponse : IMessage, new()
        {
            var requestObject = new RequestObject
            {
                HandlerId = name,
                Request = request
            };

            return (TResponse)await _requestsQueue.Enqueue(requestObject);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var valueTuple in _externalEventHandlersDictionary)
                {
                    valueTuple.Value.ExternalEvent.Dispose();
                }
                _externalEventHandlersDictionary = null;
            }
        }
    }
}
