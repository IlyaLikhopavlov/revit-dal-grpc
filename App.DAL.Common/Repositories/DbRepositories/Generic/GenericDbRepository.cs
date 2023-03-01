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
        protected readonly ProjectsDataContext DbContext;

        private readonly IEntityConverter<TModelItem, TDbEntity> _entityConverter;

        protected readonly DbSet<TDbEntity> DbSet;

        private readonly DocumentDescriptor _documentDescriptor;

        public GenericDbRepository(
            IDbContextFactory<ProjectsDataContext> dbContextFactory,
            IEntityConverter<TModelItem, TDbEntity> entityConverter,
            DocumentDescriptor documentDescriptor)
        {
            DbContext = dbContextFactory.CreateDbContext();
            DbSet = DbContext.Set<TDbEntity>();
            _documentDescriptor = documentDescriptor;
            _entityConverter = entityConverter;
            Initialization = Task.CompletedTask;
        }

        protected virtual IQueryable<TDbEntity> Query(bool eager = false)
        {
            var query = DbSet.AsQueryable();
            
            if (!eager)
            {
                return query;
            }

            var navigationProperties = DbContext.Model.FindEntityType(typeof(TDbEntity))?
                .GetDerivedTypesInclusive()
                .SelectMany(type => type.GetNavigations())
                .Distinct();

            return navigationProperties?
                .Aggregate(query, (current, property) => 
                    current.Include(property.Name));
        }

        public Task Initialization { get; }

        public virtual IEnumerable<TModelItem> GetAll()
        {
            var result = Query(true)
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .Select(x => _entityConverter.ConvertToModel(x))
                    .ToList();

            return result;
        }

        public virtual TModelItem GetById(int elementId)
        {
            var entity =
                Query(true)
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .First(x => x.Id == elementId);

            return _entityConverter.ConvertToModel(entity);
        }

        public virtual void Insert(TModelItem element)
        {
            element.Id = 0;

            var project = DbContext.Projects.First(x => x.UniqueId == _documentDescriptor.Id);

            var entity = _entityConverter.ConvertToEntity(element);
            entity.ProjectId = project.Id;

            DbSet.Add(entity);
        }

        public virtual void Remove(int elementId)
        {
            var entity = 
                DbSet
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .First(x => x.Id == elementId);

            DbSet.Remove(entity);
        }

        public virtual void Update(TModelItem element)
        {
            var entity = DbSet
                .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                .First(x => x.Id == element.Id);

            _entityConverter.UpdateEntity(element, ref entity);

            DbSet.Update(entity);
        }

        public virtual async Task SaveAsync()
        {
            _ = await DbContext.SaveChangesAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
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
