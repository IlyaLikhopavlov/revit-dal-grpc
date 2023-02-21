using App.DML;

namespace App.DAL.Common.Repositories
{
    public interface IBarRepository : IRepository<Bar>
    {
        void Insert(Bar bar, Category obtainedCategory);
    }
}
