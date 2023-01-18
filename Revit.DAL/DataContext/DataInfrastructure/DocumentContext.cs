using Bimdance.Framework.Exceptions;
using Element = App.DML.Element;

namespace App.DAL.DataContext.DataInfrastructure
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

        protected void Initialize()
        {
            SetSets();
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
            var requiredSet = RevitSets.FirstOrDefault(x => x.InternalEntityType == typeof(TEntity));

            if (requiredSet == null)
            {
                throw new RevitDataAccessException($"Set for entity type {typeof(TEntity)} not founded in the Revit context");
            }

            return (EntityProxy<TEntity>)requiredSet.GetEntry(entity.Id);
        }

        public void PullSets()
        {
            foreach (var revitSet in RevitSets)
            {
                revitSet.PullRevitEntities();
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
