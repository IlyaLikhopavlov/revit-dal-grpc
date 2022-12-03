using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DAL.DataContext.RevitSets;

namespace Revit.DAL.DataContext
{
    public class DataContext : DocumentContext, IDataContext
    {
        public DataContext(
            IFactory<Document, FooSet> foos,
            IFactory<Document, BarSet> bars,
            Document document) : base(document)
        {
            Foo = foos.New(document);
            Bar = bars.New(document);
            Initialize();
        }

        public FooSet Foo { get; }

        public BarSet Bar { get; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void ResolveForeignRelations()
        {
        }

        public void Dispose()
        {
        }
    }
}
