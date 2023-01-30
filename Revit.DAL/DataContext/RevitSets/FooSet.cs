using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Revit.DataContext.RevitSets
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
