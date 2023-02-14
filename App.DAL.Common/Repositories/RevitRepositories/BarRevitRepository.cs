using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DML;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class BarRevitRepository : GenericRevitRepository<Bar>, IBarRepository
    {
        public BarRevitRepository(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory, 
            DocumentDescriptor documentDescriptor) 
            : base(documentDescriptorServiceScopeFactory, documentDescriptor)
        {
        }
    }
}
