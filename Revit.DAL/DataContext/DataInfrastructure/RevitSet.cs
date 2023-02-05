using System.Collections;
using App.DAL.Revit.Converters.Common;
using App.DAL.Revit.DataContext.DataInfrastructure.Enums;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.Exceptions;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public abstract class RevitSet<TModelElement> : IRevitSet<TModelElement>, ISynchronizable
        where TModelElement : Element
    {
        private readonly Queue<EntityProxy<TModelElement>> _addBuffer = new();

        protected IDictionary<int, EntityProxy<TModelElement>> EntityProxiesDictionary = new Dictionary<int, EntityProxy<TModelElement>>();

        private readonly RevitInstanceConverter<TModelElement> _converter;

        protected DocumentDescriptor DocumentDescriptor;

        protected RevitSet(
            DocumentDescriptor documentDescriptor,
            IFactory<DocumentDescriptor, RevitInstanceConverter<TModelElement>> converterFactory)
        {
            DocumentDescriptor = documentDescriptor;
            _converter = converterFactory.New(DocumentDescriptor);
        }
        
        public Type InternalEntityType => typeof(TModelElement);

        public IEnumerable<object> Entities => Entries.Select(x => x.Entity)!;

        public IEnumerable<EntityProxy<TModelElement>> Entries => EntityProxiesDictionary.Values.Concat(_addBuffer);

        public virtual async Task PullRevitEntities()
        {
            EntityProxiesDictionary = (await _converter.PullWholeFromRevit())
                .ToDictionary(x => x.Id, x => new EntityProxy<TModelElement>(x, x.Id));
        }

        public TModelElement Find(int keyValue)
        {
            return EntityProxiesDictionary.TryGetValue(keyValue, out var entityProxy) 
                ? entityProxy.Entity 
                : throw new InvalidOperationException($"Object with provided ID={keyValue} hasn't found.");
        }

        public TModelElement Remove(int keyValue)
        {
            if (!EntityProxiesDictionary.TryGetValue(keyValue, out var entityProxy))
            {
                throw new InvalidOperationException($"Object with provided ID={keyValue} hasn't found.");
            }

            entityProxy.EntityState = EntityState.Deleted;
            return entityProxy.Entity;
        }

        public TModelElement Attach(TModelElement entity)
        {
            if (EntityProxiesDictionary.ContainsKey(entity.Id))
            {
                throw new RevitDataAccessException($"Entity with ID={entity.Id} can't be attached to the {typeof(TModelElement)} set. Provided ID already exists.");
            }

            EntityProxiesDictionary.Add(
                entity.Id,
                new EntityProxy<TModelElement>(entity, entity.Id, EntityState.Attached));

            return entity;
        }

        public TModelElement Add(TModelElement entity)
        {
            _addBuffer.Enqueue(new EntityProxy<TModelElement>(entity, -1, EntityState.Added));
            return entity;
        }

        public IEnumerator<TModelElement> GetEnumerator()
        {
            return Entries.Select(x => x.Entity).GetEnumerator()!;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public async Task Sync()
        {
            var listToSync = new List<EntityProxy<TModelElement>>(EntityProxiesDictionary.Values);

            while (_addBuffer.Count > 0)
            {
               await SyncAdded(_addBuffer.Dequeue());
            }

            foreach (var entityProxy in listToSync)
            {
                switch (entityProxy.EntityState)
                {
                    case EntityState.Added:
                        break;
                    case EntityState.Deleted:
                        await SyncDeleted(entityProxy);
                        break;
                    case EntityState.Modified:
                        await SyncModified(entityProxy);
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Attached:
                        await SyncModified(entityProxy);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async Task SyncAdded(EntityProxy<TModelElement> entityProxy)
        {
            var id = await _converter.CreateRevitElement();

            entityProxy.Entity.Id = id;
            entityProxy.Id = id;
            entityProxy.EntityState = EntityState.Unchanged;

            await _converter.PushToRevit(entityProxy.Entity);

            EntityProxiesDictionary.Add(entityProxy.Entity.Id, entityProxy);
        }

        protected async Task SyncDeleted(EntityProxy<TModelElement> entityProxy)
        {
            await _converter.DeleteRevitElement(entityProxy.Id);

            EntityProxiesDictionary.Remove(entityProxy.Id);
        }

        protected async Task SyncModified(EntityProxy<TModelElement> entityProxy)
        {
            entityProxy.EntityState = EntityState.Unchanged;

            await _converter.PushToRevit(entityProxy.Entity);
        }

        public object GetEntity(int id)
        {
            return Find(id);
        }

        public object GetEntry(int id)
        {
            return Entries.First(x => x.Entity.Id == id)!;
        }
    }
}
