using Autodesk.Revit.UI;
using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public abstract class ExternalServiceEventHandler<TRequest, TResponse> : IExternalServiceEventHandler
        where TRequest : IMessage
        where TResponse : IMessage
    {
        protected TRequest Request;

        private RequestObject _requestObject;

        public ExternalEvent ExternalEvent { get; set; }

        public void SetRequest(RequestObject request)
        {
            _requestObject = request;
            Request = (TRequest)_requestObject.Request;
            ExternalEvent?.Raise();
        }

        protected abstract TResponse Execute(Autodesk.Revit.DB.Document document);

        public void Execute(UIApplication app)
        {
            if (Request is null)
            {
                throw new InvalidOperationException(@"Request must be initialized before.");
            }

            try
            {
                var response = Execute(app.ActiveUIDocument.Document);
                _requestObject.SetResult(response);
            }
            catch (Exception ex)
            {
                _requestObject.SetException(ex);
            }
            finally
            {
                Request = default;
            }
        }
        
        public string GetName() => GetType().Name;
    }
}
