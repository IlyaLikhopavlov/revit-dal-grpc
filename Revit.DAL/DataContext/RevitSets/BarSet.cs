using App.DAL.Converters;
using App.DAL.Converters.Common;
using App.DAL.DataContext.DataInfrastructure;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DML;

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
