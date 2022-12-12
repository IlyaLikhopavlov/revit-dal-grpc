using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Grpc.Core;
using Revit.Services.Processing;

namespace Revit.Services.Grpc.Services
{
    public class RevitActiveDocumentNotificationService : RevitActiveDocumentNotification.RevitActiveDocumentNotificationBase
    {
        private readonly BlockingCollection<Document> _documents = new();

        private readonly ApplicationProcessing _applicationProcessing;

        public RevitActiveDocumentNotificationService(ApplicationProcessing applicationProcessing)
        {
            _applicationProcessing = applicationProcessing;
            _applicationProcessing.DocumentChanged += DocumentChanged;
        }

        private void DocumentChanged(object sender, Processing.EventArgs.DocumentChangedEventArgs e)
        {
            if (e.ActiveDocument is null)
            {
                return;
            }
            
            _documents.Add(e.ActiveDocument);
        }

        private static DocumentDescriptor Convert(Document document)
        {
             return 
                document.IsFamilyDocument
                ?
                    new DocumentDescriptor
                    {
                        Id = string.Empty,
                        Title = string.Empty
                    }
                :
                    new DocumentDescriptor
                    {
                        Id = document.ProjectInformation?.UniqueId ?? string.Empty,
                        Title = document.Title ?? string.Empty
                    };
        }

        public override async Task OnDocumentChanged(
            EmptyRequest request, 
            IServerStreamWriter<OnDocumentChangedResponse> responseStream, 
            ServerCallContext context)
        {
            var currentDocument = _applicationProcessing.UiApplication?.ActiveUIDocument?.Document;

            if (currentDocument is not null && !_documents.Any())
            {
                _documents.Add(currentDocument);
            }
            
            foreach (var document in _documents.GetConsumingEnumerable(context.CancellationToken))
            {
                await responseStream.WriteAsync(new OnDocumentChangedResponse { ActiveDocument = Convert(document) });
            }
        }
    }
}
