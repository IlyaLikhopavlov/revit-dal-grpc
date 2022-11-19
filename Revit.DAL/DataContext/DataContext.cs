using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Constants;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DAL.DataContext.RevitSets;
using Revit.DAL.Utils;

namespace Revit.DAL.DataContext
{
    public class DataContext : DocumentContext, IDataContext
    {
        public DataContext(
            IFactory<Document, FooSet> foos,
            IFactory<Document, BarSet> bars,
            Document document)
            : base(document)
        {
            
            Foo = foos.New(document);
            Bar = bars.New(document);
            Initialize();
        }

        public FooSet Foo { get; }

        public BarSet Bar { get; }

        protected sealed override void Initialize()
        {
            SetSets();
            base.Initialize();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected void SetSets()
        {
            RevitSets = typeof(DataContext)
                .GetProperties()
                .Where(p => p.PropertyType.GetInterfaces().Contains(typeof(IRevitSet)))
                .Select(p => p.GetValue(this, null) as IRevitSet)
                .Where(x => x != null)
                .ToList();
        }

        protected override void ResolveForeignRelations()
        {
            
        }

        public void Dispose()
        {
            //if (AutomationNetworks.Entries.First().EntityState == EntityState.Modified)
            //{
            //    SaveChanges();
            //}
        }
    }
}
