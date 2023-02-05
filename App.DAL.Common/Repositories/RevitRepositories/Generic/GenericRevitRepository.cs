using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.DataInfrastructure.Enums;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.Initialization;

namespace App.DAL.Common.Repositories.RevitRepositories.Generic
{
    public class GenericRevitRepository<T> : IRepository<T>, IAsyncInitialization
        where T : Element
    {
        private readonly IDataContext _context;

        private IRevitSet<T> _revitSet;

        public GenericRevitRepository(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory,
            DocumentDescriptor documentDescriptor)
        {
            _context = dataContextFactory.New(documentDescriptor);
            Initialization = InitializeAsync();

        }

        private async Task InitializeAsync()
        {
            await _context.Initialization;
            _revitSet = (IRevitSet<T>)_context.GetRevitSet(typeof(T));
        }

        public Task Initialization { get; }

        public IEnumerable<T> GetAll()
        {
            return _revitSet;
        }

        public T GetById(int elementId)
        {
            return _revitSet.First(x => x.Id == elementId);
        }

        public void Insert(T element)
        {
            _revitSet.Add(element);
        }

        public void Remove(int elementId)
        {
            _revitSet.Remove(elementId);
        }

        public void Update(T element)
        {
            var entity = _revitSet.Entries.First(x => x.Id == element.Id);
            entity.Entity = element;
            entity.EntityState = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
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
