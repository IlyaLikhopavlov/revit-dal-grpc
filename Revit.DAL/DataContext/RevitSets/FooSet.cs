using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DML;

namespace App.DAL.Revit.DataContext.RevitSets
{
    public class FooSet : RevitSet<Foo>
    {
        public FooSet(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory,
            DocumentDescriptor documentDescriptor) :
            base(documentDescriptorServiceScopeFactory, documentDescriptor)
        {
        }
    }
}
