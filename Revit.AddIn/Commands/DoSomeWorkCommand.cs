using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.AddIn.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DoSomeWorkCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var document = commandData.Application.ActiveUIDocument.Document;

            //var scopeFactory = RevitAndMdiApp.ServiceProvider.GetService<IDocumentServiceScopeFactory>();
            //var documentScope = scopeFactory?.CreateDocumentScope(document);
            //var docDependentEntity = documentScope?
            //    .ServiceProvider
            //    .GetService<IFactory<Document, DocDependentEntity>>()
            //    ?.New(document);

            //docDependentEntity?.ShowIdentityInRevit();

            return Result.Succeeded;
        }
    }
}
