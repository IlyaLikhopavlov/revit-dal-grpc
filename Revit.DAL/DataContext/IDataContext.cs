using App.DAL.DataContext.RevitSets;
using Bimdance.Framework.Initialization;

namespace App.DAL.DataContext
{
    public interface IDataContext : IAsyncInitialization
    {
        FooSet Foo { get; }

        BarSet Bar { get; }

        Task SaveChanges();
    }
}
