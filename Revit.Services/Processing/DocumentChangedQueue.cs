using System.Collections.Concurrent;
using Autodesk.Revit.DB;

namespace Revit.Services.Processing
{
    internal class DocumentChangedQueue
    {
        private BlockingCollection<Document> _documents = new();

        public void Enqueue(Document document)
        {
            _documents.Add(document);
        }

        private async void OnStart()
        {
            foreach (var document in _documents.GetConsumingEnumerable())
            {
                
            }
        }
    }
}
