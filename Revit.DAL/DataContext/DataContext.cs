using App.DAL.DataContext.DataInfrastructure;
using App.DAL.DataContext.RevitSets;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.Initialization;

namespace App.DAL.DataContext
{
    public class DataContext : DocumentContext, IDataContext
    {
        public DataContext(
            IFactory<DocumentDescriptor, FooSet> foosFactory,
            IFactory<DocumentDescriptor, BarSet> barsFactory,
            DocumentDescriptor documentDescriptor) : 
            base(documentDescriptor)
        {
            Foo = foosFactory.New(documentDescriptor);
            Bar = barsFactory.New(documentDescriptor);
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

        public override void Dispose()
        {
        }
    }
}
