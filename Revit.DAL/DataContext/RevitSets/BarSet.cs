using App.DAL.DataContext.DataInfrastructure;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Constants;
using Revit.DAL.Converters.Common;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DML;
using Revit.DAL.Storage.Infrastructure;

namespace Revit.DAL.DataContext.RevitSets
{
    public class BarSet : RevitSet<Bar, FamilyInstance>
    {
        public BarSet(
            IFactory<Document, RevitInstanceConverter<Bar, FamilyInstance>> converterFactory, 
            IFactory<Document, ISchemaDescriptorsRepository> schemaDescriptorsRepository,
            Document document) :
            base(document, schemaDescriptorsRepository, converterFactory)
        {
        }

        protected override FamilyInstance CreateRevitElement(Bar modelElement)
        {
            throw new NotImplementedException();
        }
    }
}
