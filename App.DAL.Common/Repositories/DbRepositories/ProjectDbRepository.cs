using App.DAL.Db;
using App.DAL.Db.Mapping;
using App.DML;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class ProjectDbRepository : IProjectRepository
    {
        private readonly ProjectsDataContext _dbContext;

        public ProjectDbRepository(
            IDbContextFactory<ProjectsDataContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public IEnumerable<Project> GetAll()
        {
            return _dbContext.Projects
                .Select(x => x.ProjectEntityToProject())
                .ToList();
        }

        public Project GetById(int elementId)
        {
            return _dbContext.Projects
                .First(x => x.Id == elementId)
                .ProjectEntityToProject();
        }

        public void Insert(Project element)
        {
            var entity = element.ProjectToProjectEntity();

            _dbContext.Projects.Add(entity);
        }

        public void Remove(int elementId)
        {
            var entity = _dbContext.Projects.First(x => x.Id == elementId);

            _dbContext.Projects.Remove(entity);
        }

        public void Update(Project element)
        {
            var entity = _dbContext.Projects
                .First(x => x.Id == element.Id);

            entity.UpdateProjectEntityByProject(element);

            _dbContext.Projects.Update(entity);
        }

        public Project GetByUniqueId(string uniqueId)
        {
            return _dbContext.Projects
                .First(x => x.UniqueId == uniqueId)
                .ProjectEntityToProject();
        }

        public async Task SaveAsync()
        {
            _ = await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
