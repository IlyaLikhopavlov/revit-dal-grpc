using App.DAL.Converters;
using App.DAL.Converters.Common;
using App.DAL.DataContext.DataInfrastructure;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.DataContext.RevitSets
{
    public class BarSet : RevitSet<Bar>
    {
        public BarSet(
            IFactory<DocumentDescriptor, BarConverter> converterFactory,
            DocumentDescriptor documentDescriptor) :
            base(documentDescriptor, converterFactory)
        {
        }
    }
}
