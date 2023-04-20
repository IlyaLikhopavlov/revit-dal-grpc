using App.DAL.Db;
using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using Microsoft.EntityFrameworkCore;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class ProjectDbRepository : IProjectRepository
    {
        private readonly ProjectsDataContext _dbContext;

        private readonly IEntityConverter<Project, ProjectEntity> _projectConverter;

        public ProjectDbRepository(
            IEntityConverter<Project, ProjectEntity> projectConverter,
            ProjectsDataContext dbContext)
        {
            _dbContext = dbContext;
            _projectConverter = projectConverter;
            Initialization = Task.CompletedTask;
        }

        public Task Initialization { get; }

        public IEnumerable<Project> GetAll()
        {
            return _dbContext.Projects
                .Select(x => _projectConverter.ConvertToModel(x))
                .ToList();
        }

        public Project GetById(int elementId)
        {
            var project = _dbContext.Projects
                .First(x => x.Id == elementId);

            return _projectConverter.ConvertToModel(project);
        }

        public void Insert(Project element)
        {
            var project = _projectConverter.ConvertToEntity(element);

            _dbContext.Projects.Add(project);
        }

        public void Remove(int elementId)
        {
            var project = _dbContext.Projects.First(x => x.Id == elementId);

            _dbContext.Projects.Remove(project);
        }

        public void Update(Project element)
        {
            var entity = _dbContext.Projects
                .First(x => x.Id == element.Id);

            _projectConverter.UpdateEntity(element, ref entity);

            _dbContext.Projects.Update(entity);
        }

        public bool Contains(int elementId)
        {
            return _dbContext.Projects.FirstOrDefault(x => x.Id == elementId) is not null;
        }

        public Project GetByUniqueId(string uniqueId)
        {
            var project = _dbContext.Projects
                .First(x => x.UniqueId == uniqueId);

            return _projectConverter.ConvertToModel(project);
        }

        public async Task SaveAsync()
        {
            _ = await _dbContext.SaveChangesAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
