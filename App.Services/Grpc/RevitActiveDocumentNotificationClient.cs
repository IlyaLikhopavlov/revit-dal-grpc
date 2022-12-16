using App.Grpc.Bundle.ScopedServicesFunctionality;
using App.Services.Revit;
using Grpc.Core;
using Revit.Services.Grpc.Services;

namespace App.Services.Grpc
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
                    }
                    else
                    {
                        _scopeFactory.OnDocumentClosing(descriptor);
                        continue;
                    }
                    
                    if (string.IsNullOrWhiteSpace(_revitApplication.ActiveDocument.Id))
                    {
                        Console.WriteLine($"- no data {_revitApplication.DataStatus}");
                        continue;
                    }

                    Console.WriteLine($"Document - {_revitApplication.ActiveDocument.Title} Data status - {_revitApplication.DataStatus} " + 
                                      $"Document Action - {_revitApplication.ActiveDocument.DocumentAction}");
                }

            }
            catch (Exception ex)
            {
                _revitApplication.SetDataStatusUntrusted();
                Console.WriteLine($"Revit communication failed - {ex}");
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
