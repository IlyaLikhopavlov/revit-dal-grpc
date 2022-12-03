using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality;
using Microsoft.Extensions.DependencyInjection;
using Revit.DAL.DataContext;
using Revit.DAL.Utils;

namespace Revit.AddIn.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ReadDataCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var document = commandData.Application.ActiveUIDocument.Document;

            using var scopeFactory = RevitDalApp.ServiceProvider.GetService<IDocumentServiceScopeFactory>();
            var documentScope = scopeFactory?.CreateDocumentScope(document);
            var dataContext = documentScope?
                .ServiceProvider
                .GetService<IFactory<Document, IDataContext>>()
                ?.New(document);

            //Getting all entities
            var entities = dataContext.GetBaseEntityProxies();

            TaskDialog.Show(
                "Info",
                $"{string.Join(", ", entities.Select(x => $"[{x.BaseEntity.Id},{x.BaseEntity.Name}]"))}");

            //Getting Foos
            var foos = dataContext?.Foo.Entries.Select(x => x.Entity);

            TaskDialog.Show(
                "Info",
                $"{string.Join(", ", foos?.Select(x => $"[{x.Id},{x.Name}]") ?? new[] {string.Empty})}");

            return Result.Succeeded;
        }
    }
}
