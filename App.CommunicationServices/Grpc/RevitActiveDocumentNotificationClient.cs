using App.CommunicationServices.Revit;
using App.CommunicationServices.Revit.EventArgs;
using App.ScopedServicesFunctionality;
using Grpc.Core;
using Revit.Services.Grpc.Services;

namespace App.CommunicationServices.Grpc
{
    public class RevitActiveDocumentNotificationClient : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly RevitApplication _revitApplication;

        private readonly RevitActiveDocumentNotification.RevitActiveDocumentNotificationClient _client;

        private readonly IDocumentDescriptorServiceScopeFactory _scopeFactory;

        public RevitActiveDocumentNotificationClient(
            RevitApplication revitApplication,
            IDocumentDescriptorServiceScopeFactory scopeFactory)
        {
            _revitApplication = revitApplication;
            
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
                        _revitApplication.ActiveDocument = descriptor;
                        _revitApplication.DocumentDescriptorChanged?.Invoke(
                            this, 
                            new DocumentDescriptorChangedEventArgs { DocumentDescriptor = descriptor });
                    }
                    else
                    {
                        _scopeFactory.OnDocumentClosing(descriptor);
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(_revitApplication.ActiveDocument.Id))
                    {
                        continue;
                    }

                    _revitApplication.DocumentDescriptorChanged?.Invoke(
                        this, 
                        new DocumentDescriptorChangedEventArgs { DocumentDescriptor = null });
                }

            }
            catch (Exception)
            {
                _revitApplication.SetDataStatusUntrusted();
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
