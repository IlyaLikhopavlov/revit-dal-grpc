using Revit.DAL.DataContext.RevitSets;

namespace Revit.DAL.DataContext
{
    public interface IDataContext
    {
        FooSet Foo { get; }

        BarSet Bar { get; }

        void SaveChanges(bool isInSubTransaction = false);
    }
}
