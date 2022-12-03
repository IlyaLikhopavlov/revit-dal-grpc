using Autodesk.Revit.DB;
using Revit.DAL.Constants;
using Revit.DAL.Exceptions;
using Element = Revit.DML.Element;

namespace Revit.DAL.DataContext.DataInfrastructure
{
    public abstract class DocumentContext : IDisposable
    {
        protected readonly Document Document;

        protected List<IRevitSet> RevitSets;

        protected Guid ContextGuid { get; } = Guid.NewGuid();

        protected DocumentContext(Document document)
        {
            Document = document ?? throw new ArgumentException($"{nameof(document)} isn't initialized.");
        }

        protected void Initialize()
        {
            SetSets();
            var modelBuilder = new ModelBuilder(RevitSets);
            OnModelCreating(modelBuilder);
            modelBuilder.Build();
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

        public void SaveChanges(bool isInSubTransaction = false)
        {
            if (Document.IsReadOnly)
            {
                throw new InvalidOperationException($"Document {Document.Title} is read only. Changes can't be saved.");
            }

            if (!Document.IsModifiable && !isInSubTransaction)
            {
                using var transaction = new Transaction(Document, RevitStorage.SaveChangesTransactionName);
                try
                {
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        Sync();
                    }

                    if (TransactionStatus.Committed != transaction.Commit())
                    {
                        transaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    transaction.RollBack();
                    throw;
                }
            }
            else
            {
                using var subTransaction = new SubTransaction(Document);
                try
                {
                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        Sync();
                    }

                    if (TransactionStatus.Committed != subTransaction.Commit())
                    {
                        subTransaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    subTransaction.RollBack();
                    throw;
                }
            }
        }

        protected abstract void OnModelCreating(ModelBuilder modelBuilder);

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

        protected void Sync()
        {
            var syncSets = RevitSets.Cast<ISynchronizable>();

            foreach (var syncSet in syncSets)
            {
                syncSet.Sync();
            }
        }

        public virtual void Dispose()
        {
        }
    }
}
