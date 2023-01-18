using App.DAL.Converters;
using App.DAL.Converters.Common;
using App.DAL.DataContext.DataInfrastructure;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.DataContext.RevitSets
{
    public class FooSet : RevitSet<Foo>
    {
        public FooSet(
            IFactory<DocumentDescriptor, FooConverter> converterFactory,
            DocumentDescriptor documentDescriptor) :
            base(documentDescriptor, converterFactory)
        {
        }
    }
}
