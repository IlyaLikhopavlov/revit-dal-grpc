using App.CommunicationServices.Revit;
using App.CommunicationServices.Revit.EventArgs;
using Grpc.Core;
using Revit.Services.Grpc.Services;
using IDocumentDescriptorServiceScopeFactory = App.CommunicationServices.ScopedServicesFunctionality.IDocumentDescriptorServiceScopeFactory;

namespace App.CommunicationServices.Grpc
{
    public class RevitActiveDocumentNotificationClient : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly ApplicationObject _applicationObject;

        private readonly RevitActiveDocumentNotification.RevitActiveDocumentNotificationClient _client;

        private readonly IDocumentDescriptorServiceScopeFactory _scopeFactory;

        public RevitActiveDocumentNotificationClient(
            ApplicationObject applicationObject,
            IDocumentDescriptorServiceScopeFactory scopeFactory)
        {
            _applicationObject = applicationObject;
            
            var channel = new Channel("127.0.0.1:5005", ChannelCredentials.Insecure);
            _client = new RevitActiveDocumentNotification.RevitActiveDocumentNotificationClient(channel);
            _scopeFactory = scopeFactory;
        }

        public async Task RunGettingRevitNotification()
        {
            try
            {
                var call = _client.OnDocumentChanged(new EmptyRequest(), null, null, _cancellationTokenSource.Token);
                while (await call.ResponseStream.MoveNext())
                {
                    var descriptor = call.ResponseStream.Current.DocumentDescriptor;

                    if (descriptor.DocumentAction == DocumentActionEnum.Activated)
                    {
                        _applicationObject.ActiveDocument = descriptor;
                        _applicationObject.DocumentDescriptorChanged?.Invoke(
                            this, 
                            new DocumentDescriptorChangedEventArgs { DocumentDescriptor = descriptor });
                    }
                    else
                    {
                        _scopeFactory.RemoveScope(descriptor);
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(_applicationObject.ActiveDocument.Id))
                    {
                        continue;
                    }

                    _applicationObject.DocumentDescriptorChanged?.Invoke(
                        this, 
                        new DocumentDescriptorChangedEventArgs { DocumentDescriptor = null });
                }

            }
            catch (Exception)
            {
                _applicationObject.SetDataStatusUntrusted();
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
