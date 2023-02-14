using App.DAL.Db;
using App.DAL.Db.Mapping.Abstractions;
using App.DAL.Db.Model;
using App.DML;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.Repositories.DbRepositories.Generic
{
    public class GenericDbRepository<TModelItem, TDbEntity> : IRepository<TModelItem> 
        where TModelItem : Element
        where TDbEntity : BaseEntity
    {
        private readonly ProjectsDataContext _dbContext;

        private readonly IEntityConverter<TModelItem, TDbEntity> _entityConverter;

        private readonly DbSet<TDbEntity> _dbSet;

        private readonly DocumentDescriptor _documentDescriptor;

        public GenericDbRepository(
            IDbContextFactory<ProjectsDataContext> dbContextFactory,
            IEntityConverter<TModelItem, TDbEntity> entityConverter,
            DocumentDescriptor documentDescriptor)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _dbSet = _dbContext.Set<TDbEntity>();
            _documentDescriptor = documentDescriptor;
            _entityConverter = entityConverter;
            Initialization = Task.CompletedTask;
        }

        public Task Initialization { get; }

        public IEnumerable<TModelItem> GetAll()
        {
            return
                _dbSet
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .Select(x => _entityConverter.ConvertToModel(x))
                    .ToList();
        }

        public TModelItem GetById(int elementId)
        {
            var entity =
                _dbSet
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .First(x => x.Id == elementId);

            return _entityConverter.ConvertToModel(entity);
        }

        public void Insert(TModelItem element)
        {
            var project = _dbContext.Projects.First(x => x.UniqueId == _documentDescriptor.Id);

            var entity = _entityConverter.ConvertToEntity(element);
            entity.ProjectId = project.Id;

            _dbSet.Add(entity);
        }

        public void Remove(int elementId)
        {
            var entity = 
                _dbSet
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .First(x => x.Id == elementId);

            _dbSet.Remove(entity);
        }

        public void Update(TModelItem element)
        {
            var entity = _dbSet
                .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                .First(x => x.Id == element.Id);

            _entityConverter.UpdateEntity(element, ref entity);

            _dbSet.Update(entity);
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
