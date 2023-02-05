using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DAL.Revit.DataContext;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class FooRevitRepository : GenericRevitRepository<Foo>, IFooRepository
    {
        public FooRevitRepository(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory, 
            DocumentDescriptor documentDescriptor) 
            : base(dataContextFactory, documentDescriptor)
        {
        }
    }
}
