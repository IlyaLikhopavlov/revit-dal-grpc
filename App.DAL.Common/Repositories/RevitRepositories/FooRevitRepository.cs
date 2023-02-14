using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DML;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class FooRevitRepository : GenericRevitRepository<Foo>, IFooRepository
    {
        public FooRevitRepository(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory, 
            DocumentDescriptor documentDescriptor) 
            : base(documentDescriptorServiceScopeFactory, documentDescriptor)
        {
        }
    }
}
