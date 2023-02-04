using App.DML;

namespace App.DAL.Common.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Project GetByUniqueId(string uniqueId);
    }
}
