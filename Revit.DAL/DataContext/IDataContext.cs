using Revit.DAL.DataContext.RevitSets;

namespace Revit.DAL.DataContext
{
    public interface IDataContext : IDisposable
    {
        FooSet Foo { get; }

        BarSet Bar { get; }

        void SaveChanges(bool isInSubtransaction = false);
    }
}
