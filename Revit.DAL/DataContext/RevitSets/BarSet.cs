using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DML;

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
