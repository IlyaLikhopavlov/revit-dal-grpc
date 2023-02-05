using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DAL.Revit.DataContext;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class BarRevitRepository : GenericRevitRepository<Bar>, IBarRepository
    {
        public BarRevitRepository(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory, 
            DocumentDescriptor documentDescriptor) 
            : base(dataContextFactory, documentDescriptor)
        {
        }
    }
}
