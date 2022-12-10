using Google.Protobuf;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public class RequestObject
    {
        private readonly TaskCompletionSource<IMessage> _taskCompletionSource;

        public RequestObject()
        {
            _taskCompletionSource = new TaskCompletionSource<IMessage>();
        }

        public IMessage Request { get; set; }

        public string HandlerId { get; set; }

        public void SetResult(IMessage result)
        {
            _taskCompletionSource.SetResult(result);
        }

        public void SetException(Exception exception)
        {
            _taskCompletionSource.SetException(exception);
        }

        public Task<IMessage> GetResponse()
        {
            return _taskCompletionSource.Task;
        }
    }
}
