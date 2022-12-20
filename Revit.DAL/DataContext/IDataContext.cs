using App.DAL.DataContext.RevitSets;

namespace App.DAL.DataContext
{
    public interface IDataContext
    {
        FooSet Foo { get; }

        BarSet Bar { get; }

        Task SaveChanges();
    }
}
