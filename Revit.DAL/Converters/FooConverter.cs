using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class FooConverter : RevitInstanceConverter<Foo, FamilyInstance>
    {
        public FooConverter(
            IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document) : 
            base(extensibleStorageFactory, document)
        {
        }

        protected override void PushParametersToRevit(FamilyInstance revitElement, Foo modelElement)
        {
        }

        protected override void PullParametersFromRevit(FamilyInstance revitElement, ref Foo modelElement)
        {
        }
    }
}
