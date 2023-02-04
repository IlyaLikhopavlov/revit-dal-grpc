using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Grpc.Core;
using Revit.Services.Processing;
using DocumentClosingEventArgs = Revit.Services.Processing.EventArgs.DocumentClosingEventArgs;

namespace Revit.Services.Grpc.Services
{
    public class RevitActiveDocumentNotificationService : RevitActiveDocumentNotification.RevitActiveDocumentNotificationBase
    {
        private readonly BlockingCollection<(Document source, DocumentDescriptor descriptor)> _documents = new();

        private readonly ApplicationProcessing _applicationProcessing;

        public RevitActiveDocumentNotificationService(ApplicationProcessing applicationProcessing)
        {
            _applicationProcessing = applicationProcessing;
            _applicationProcessing.DocumentChanged += OnDocumentChanged;
            _applicationProcessing.DocumentClosing += OnDocumentClosing;
        }

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.ClosingDocument is null)
            {
                return;
            }

            _documents.Add((e.ClosingDocument, Convert(e.ClosingDocument, DocumentActionEnum.Closed)));
        }

        private void OnDocumentChanged(object sender, Processing.EventArgs.DocumentChangedEventArgs e)
        {
            if (e.ActiveDocument is null)
            {
                return;
            }

            _documents.Add((e.ActiveDocument, Convert(e.ActiveDocument)));
        }

        private static DocumentDescriptor Convert(Document document, DocumentActionEnum documentAction = DocumentActionEnum.Activated)
        {
             return 
                document.IsFamilyDocument
                ?
                    new DocumentDescriptor
                    {
                        Id = string.Empty,
                        Title = string.Empty,
                        DocumentAction = documentAction
                    }
                :
                    new DocumentDescriptor
                    {
                        Id = document.ProjectInformation?.UniqueId ?? string.Empty,
                        Title = document.Title ?? string.Empty,
                        DocumentAction = documentAction
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
                _documents.Add((currentDocument, Convert(currentDocument)));
            }
            
            foreach (var (document, descriptor) in _documents.GetConsumingEnumerable(context.CancellationToken))
            {
                await responseStream.WriteAsync(new OnDocumentChangedResponse { DocumentDescriptor = descriptor });
            }
        }
    }
}
