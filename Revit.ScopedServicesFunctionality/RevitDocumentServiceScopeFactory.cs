using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.ScopedServicesFunctionality
{
    public class RevitDocumentServiceScopeFactory : DocumentServiceScopeFactory<Document>, IRevitDocumentServiceScopeFactory
    {
        public RevitDocumentServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override IServiceScope CreateScope(Document document)
        {
            if (document.IsFamilyDocument)
            {
                return null;
            }

            var result = base.CreateScope(document);

            document.DocumentClosing += DocumentOnDocumentClosing;
            return result;

        }

        private void DocumentOnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (!ScopeDictionary.TryRemove(e.Document, out var scope))
            {
                return;
            }

            ((DocumentScope)scope).ScopeObject.DocumentClosing -= DocumentOnDocumentClosing;
        }
    }
}
