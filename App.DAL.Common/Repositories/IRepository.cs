using App.DML;
using Bimdance.Framework.Initialization;

namespace App.DAL.Common.Repositories
{
    public interface IRepository<T> : IAsyncInitialization, IDisposable where T : Element
    {
        IEnumerable<T> GetAll();
        T GetById(int elementId);
        void Insert(T element);
        void Remove(int elementId);
        void Update(T element);
        Task SaveAsync();

    }
}
