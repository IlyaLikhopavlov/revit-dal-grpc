using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.DataInfrastructure.Enums;
using App.DML;

namespace App.DAL.Common.Repositories.RevitRepositories.Generic
{
    public class GenericRevitRepository<T> : IRepository<T>
        where T : Element
    {
        private readonly IDataContext _context;

        private IRevitSet<T> _revitSet;

        private readonly DocumentDescriptor _documentDescriptor;

        public GenericRevitRepository(
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory,
            DocumentDescriptor documentDescriptor)
        {
            _context = documentDescriptorServiceScopeFactory.GetScopedService<IDataContext>();
            Initialization = InitializeAsync();
            _documentDescriptor = documentDescriptor;
        }

        private async Task InitializeAsync()
        {
            await _context.Initialization;
            _revitSet = (IRevitSet<T>)_context.GetRevitSet(typeof(T));
        }

        public Task Initialization { get; }

        public DocumentDescriptor DocumentDescriptor => new(_documentDescriptor);

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
            if (element.Id > 0)
            {
                _revitSet.Attach(element);
            }
            else
            {
                _revitSet.Add(element);
            }
            
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

        public bool Contains(int elementId)
        {
            return _revitSet.Contains(elementId);
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
                    //todo dispose here
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
