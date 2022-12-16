using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class BarConverter : RevitInstanceConverter<Bar>
    {
        public BarConverter(
            IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document) : 
            base(extensibleStorageFactory, document)
        {
        }

        protected override void PushParametersToRevit(Bar modelElement)
        {
        }

        protected override void PullParametersFromRevit(ref Bar modelElement)
        {
        }
    }
}
