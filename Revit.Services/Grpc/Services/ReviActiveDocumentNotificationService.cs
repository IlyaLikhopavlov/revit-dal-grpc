using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Grpc.Core;
using Revit.Services.Processing;

namespace Revit.Services.Grpc.Services
{
    public class RevitActiveDocumentNotificationService : RevitActiveDocumentNotification.RevitActiveDocumentNotificationBase
    {
        private readonly BlockingCollection<Document> _documents = new();

        public RevitActiveDocumentNotificationService(ApplicationProcessing applicationProcessing)
        {
            applicationProcessing.DocumentChanged += DocumentChanged;
        }

        private void DocumentChanged(object sender, Processing.EventArgs.DocumentChangedEventArgs e)
        {
            if (e.ActiveDocument is null)
            {
                return;
            }
            
            _documents.Add(e.ActiveDocument);
        }

        public override async Task OnDocumentChanged(
            EmptyRequest request, 
            IServerStreamWriter<OnDocumentChangedResponse> responseStream, 
            ServerCallContext context)
        {
            foreach (var document in _documents.GetConsumingEnumerable(context.CancellationToken))
            {
                await responseStream.WriteAsync(
                    new OnDocumentChangedResponse
                    {
                        ActiveDocument = 
                            new DocumentDescriptor
                            {
                                Id = document.ProjectInformation.UniqueId,
                                Title = document.Title
                            }
                    });
            }
        }
    }
}
