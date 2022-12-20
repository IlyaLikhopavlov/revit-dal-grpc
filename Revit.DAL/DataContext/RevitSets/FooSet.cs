using App.DAL.Converters.Common;
using App.DAL.DataContext.DataInfrastructure;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DML;

namespace App.DAL.DataContext.RevitSets
{
    public class FooSet : RevitSet<Foo>
    {
        public FooSet(
            DocumentDescriptor documentDescriptor,
            IFactory<DocumentDescriptor, RevitInstanceConverter<Foo>> converterFactory) :
            base(documentDescriptor, converterFactory)
        {
        }
    }
}
