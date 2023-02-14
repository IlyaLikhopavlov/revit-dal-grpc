using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DAL.Revit.DataContext.RevitSets;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Revit.DataContext
{
    public class DataContext : DocumentContext, IDataContext
    {
        public DataContext(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory,
            DocumentDescriptor documentDescriptor) 
            : base(documentDescriptor)
        {
            Foo = documentDescriptorServiceScopeFactory.GetScopedService<FooSet>();
            Bar = documentDescriptorServiceScopeFactory.GetScopedService<BarSet>();
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await InitializeSetsAsync();
        }

        public Task Initialization { get; }

        public FooSet Foo { get; }

        public BarSet Bar { get; }

        protected override void ResolveForeignRelations()
        {
        }
    }
}
