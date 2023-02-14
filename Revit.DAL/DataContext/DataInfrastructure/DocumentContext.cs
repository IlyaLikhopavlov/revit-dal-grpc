using Element = App.DML.Element;

namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public abstract class DocumentContext : IDisposable
    {
        private readonly DocumentDescriptor _documentDescriptor;

        protected List<IRevitSetBase> RevitSets;

        protected Guid ContextGuid { get; } = Guid.NewGuid();

        protected DocumentContext(DocumentDescriptor documentDescriptor)
        {
            _documentDescriptor = documentDescriptor;
        }

        public DocumentDescriptor DocumentDescriptor => new(_documentDescriptor);

        protected async Task InitializeSetsAsync()
        {
            SetSets();
            await PullSets();
            ResolveForeignRelations();
        }

        protected void SetSets()
        {
            RevitSets = GetType()
                .GetProperties()
                .Where(p => p.PropertyType.GetInterfaces().Contains(typeof(IRevitSetBase)))
                .Select(p => p.GetValue(this, null) as IRevitSetBase)
                .Where(x => x != null)
                .ToList();
        }

        protected abstract void ResolveForeignRelations();

        public async Task SaveChanges()
        {
            await Sync();
        }

        public EntityProxy<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : Element
        {
            var requiredSet = RevitSets.First(x => x.InternalEntityType == typeof(TEntity));

            return (EntityProxy<TEntity>)requiredSet.GetEntry(entity.Id);
        }

        public IRevitSetBase GetRevitSet(Type entityType)
        {
            return RevitSets.First(x => x.InternalEntityType == entityType);
        }

        public async Task PullSets()
        {
            foreach (var revitSet in RevitSets)
            {
                await revitSet.PullRevitEntities();
            }
        }

        protected async Task Sync()
        {
            var syncSets = RevitSets.Cast<ISynchronizable>();

            foreach (var syncSet in syncSets)
            {
                await syncSet.Sync();
            }
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
