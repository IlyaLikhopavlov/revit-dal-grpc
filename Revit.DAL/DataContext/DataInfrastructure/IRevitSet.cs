using App.DML;

namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public interface IRevitSet<T> : IRevitSetBase, IEnumerable<T>
        where T : Element
    {
        T Find(int keyValue);

        T Remove(int keyValue);

        T Attach(T entity);

        T Add(T entity);

        bool Contains(int keyValue);

        IEnumerable<EntityProxy<T>> Entries { get; }
    }
}
