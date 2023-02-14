using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Revit.DataContext.RevitSets
{
    public class BarSet : RevitSet<Bar>
    {
        public BarSet(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory,
            DocumentDescriptor documentDescriptor) :
            base(documentDescriptorServiceScopeFactory, documentDescriptor)
        {
        }
    }
}
