using App.DAL.Revit.DataContext.RevitSets;
using Bimdance.Framework.Initialization;

namespace App.DAL.Revit.DataContext
{
    public interface IDataContext : IAsyncInitialization
    {
        FooSet Foo { get; }

        BarSet Bar { get; }

        Task SaveChanges();
    }
}
