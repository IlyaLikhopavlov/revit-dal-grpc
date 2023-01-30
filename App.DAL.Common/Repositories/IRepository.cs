using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DML;

namespace App.DAL.Common.Repositories
{
    public interface IRepository<T> : IDisposable where T : BaseItem
    {
        IEnumerable<T> GetAll();
        T GetById(int elementId);
        void Insert(T element);
        void Delete(int elementId);
        void Update(T element);
        Task SaveAsync();

    }
}
