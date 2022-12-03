using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DAL.DataContext.RevitItems;
using Revit.DAL.Storage.Infrastructure;
using Revit.DML;

namespace Revit.DAL.DataContext.RevitSets
{
    public class FooSet : RevitSet<Foo, FamilyInstance>
    {
        public FooSet(
            IFactory<Document, RevitInstanceConverter<Foo, FamilyInstance>> converterFactory,
            IFactory<Document, ISchemaDescriptorsRepository> schemaDescriptorsRepository,
            Document document) :
            base(document, schemaDescriptorsRepository, converterFactory)
        {
        }

        protected override FamilyInstance CreateRevitElement(Foo modelElement)
        {
            throw new NotImplementedException();
        }
    }
}
