using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class BarConverter : RevitInstanceConverter<Bar, FamilyInstance>
    {
        public BarConverter(
            IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document) : base(extensibleStorageFactory, document)
        {
        }

        protected override void SendParametersToRevit(FamilyInstance revitElement, Bar modelElement)
        {
        }

        protected override void ReceiveParametersFromRevit(FamilyInstance revitElement, ref Bar modelElement)
        {
        }
    }
}
