using App.DAL.DataContext.DataInfrastructure;
using App.DAL.DataContext.RevitSets;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.DataContext
{
    public class DataContext : DocumentContext, IDataContext
    {
        public DataContext(
            IFactory<DocumentDescriptor, FooSet> foos,
            IFactory<DocumentDescriptor, BarSet> bars,
            DocumentDescriptor document) : 
            base(document)
        {
            Foo = foos.New(document);
            Bar = bars.New(document);
            Initialize();
        }

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
