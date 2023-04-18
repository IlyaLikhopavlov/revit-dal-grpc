using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DML;

namespace App.DAL.Common.Repositories.RevitRepositories;

public class LevelRevitRepository : GenericRevitRepository<Foo>, IFooRepository
{
    public LevelRevitRepository(
        IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory,
        DocumentDescriptor documentDescriptor)
        : base(documentDescriptorServiceScopeFactory, documentDescriptor)
    {

    }
}
