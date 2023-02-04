using Element = App.DML.Element;

namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public abstract class DocumentContext : IDisposable
    {
        protected readonly DocumentDescriptor DocumentDescriptor;

        protected List<IRevitSet> RevitSets;

        protected Guid ContextGuid { get; } = Guid.NewGuid();

        protected DocumentContext(DocumentDescriptor documentDescriptor)
        {
            DocumentDescriptor = documentDescriptor ?? throw new ArgumentException($"{nameof(documentDescriptor)} isn't initialized.");
        }

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
                .Where(p => p.PropertyType.GetInterfaces().Contains(typeof(IRevitSet)))
                .Select(p => p.GetValue(this, null) as IRevitSet)
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

        public virtual void Dispose()
        {
        }
    }
}
