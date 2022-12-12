using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Revit;
using Grpc.Core;
using Grpc.Core.Utils;
using Revit.Services.Grpc.Services;

namespace App.Services.Grpc
{
    public class RevitActiveDocumentNotificationClient : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly RevitApplication _revitApplication;

        private readonly RevitActiveDocumentNotification.RevitActiveDocumentNotificationClient _client;

        public RevitActiveDocumentNotificationClient(RevitApplication revitApplication)
        {
            _revitApplication = revitApplication;
            
            var channel = new Channel("127.0.0.1:5005", ChannelCredentials.Insecure);
            _client = new RevitActiveDocumentNotification.RevitActiveDocumentNotificationClient(channel);
        }

        public async Task RunGettingRevitNotification()
        {
            try
            {
                var call = _client.OnDocumentChanged(new EmptyRequest(), null, null, _cancellationTokenSource.Token);
                while (await call.ResponseStream.MoveNext())
                {
                    _revitApplication.ActiveDocument = call.ResponseStream.Current.ActiveDocument;
                    if (string.IsNullOrWhiteSpace(_revitApplication.ActiveDocument.Title))
                    {
                        Console.WriteLine("- no data");
                        continue;
                    }
                    Console.WriteLine(_revitApplication.ActiveDocument.Title);
                }

            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
