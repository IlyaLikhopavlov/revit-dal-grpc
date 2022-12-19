﻿using System.Collections;
using System.Reflection.Metadata;
using App.DAL.Converters.Common;
using App.Services.Grpc;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.Exceptions;
using Revit.DAL.DataContext.DataInfrastructure;
using Revit.DAL.DataContext.DataInfrastructure.Enums;
using Revit.DAL.Exceptions;
using Revit.Services.Grpc.Services;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace App.DAL.DataContext.DataInfrastructure
{
    public abstract class RevitSet<TModelElement> : IEnumerable<TModelElement>, ISynchronizable, IRevitSet
        where TModelElement : Revit.DML.Element
    {
        private readonly Queue<EntityProxy<TModelElement>> _addBuffer = new();

        protected IDictionary<int, EntityProxy<TModelElement>> EntityProxiesDictionary = new Dictionary<int, EntityProxy<TModelElement>>();

        private readonly RevitInstanceConverter<TModelElement> _converter;

        protected RevitExtraDataExchangeClient Client;

        protected DocumentDescriptor DocumentDescriptor;

        protected RevitSet(
            DocumentDescriptor documentDescriptor,
            RevitExtraDataExchangeClient client,
            IFactory<DocumentDescriptor, RevitInstanceConverter<TModelElement>> converterFactory)
        {
            DocumentDescriptor = documentDescriptor;
            _converter = converterFactory.New(DocumentDescriptor);
            Client = client;
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
            return EntityProxiesDictionary.TryGetValue(keyValue, out var entityProxy) ? entityProxy.Entity : null;
        }

        public TModelElement Remove(TModelElement entity)
        {
            if (!EntityProxiesDictionary.TryGetValue(entity.Id, out var entityProxy))
            {
                return null;
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

        public void Sync()
        {
            var listToSync = new List<EntityProxy<TModelElement>>(EntityProxiesDictionary.Values);

            while (_addBuffer.Count > 0)
            {
                SyncAdded(_addBuffer.Dequeue());
            }

            foreach (var entityProxy in listToSync)
            {
                switch (entityProxy.EntityState)
                {
                    case EntityState.Added:
                        break;
                    case EntityState.Deleted:
                        SyncDeleted(entityProxy);
                        break;
                    case EntityState.Modified:
                        SyncModified(entityProxy);
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Attached:
                        SyncModified(entityProxy);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected virtual void PushToRevit(TModelElement modelElement)
        {
            _converter?.PushToRevit(modelElement);
        }

        private void SyncAdded(EntityProxy<TModelElement> entityProxy)
        {
            var revitElement = CreateRevitElement(entityProxy.Entity);

            if (revitElement == null)
            {
                throw new RevitDataAccessException("Can't create required element");
            }

            if (!revitElement.IsValidObject)
            {
                return;
            }

            SourcesDictionary.Add(revitElement.Id.IntegerValue, revitElement);

            entityProxy.Entity.Id = revitElement.Id.IntegerValue;
            entityProxy.Id = revitElement.Id.IntegerValue;
            entityProxy.EntityState = EntityState.Unchanged;

            PushToRevit(entityProxy.Entity);

            EntityProxiesDictionary.Add(entityProxy.Entity.Id, entityProxy);
        }

        protected void SyncDeleted(EntityProxy<TModelElement> entityProxy)
        {
            var requiredId = new ElementId(entityProxy.Entity.Id);
            
            if (Document?.GetElement(requiredId) != null)
            {
                Document.Delete(requiredId);
            }

            SourcesDictionary.Remove(requiredId.IntegerValue);
            EntityProxiesDictionary.Remove(requiredId.IntegerValue);
        }

        protected void SyncModified(EntityProxy<TModelElement> entityProxy)
        {
            if (!SourcesDictionary.TryGetValue(entityProxy.Id, out var elementToModification))
            {
                elementToModification = (TRevitElement)Document?.GetElement(new ElementId(entityProxy.Id))!;
            }

            if (elementToModification == null)
            {
                throw new RevitDataAccessException($"Required element ID={entityProxy.Entity.Id} not founded in Revit. Modification failed.");
            }

            entityProxy.EntityState = EntityState.Unchanged;

            if (!elementToModification.IsValidObject)
            {
                return;
            }

            PushToRevit(elementToModification, entityProxy.Entity);
        }

        public object GetEntity(int id)
        {
            return Find(id);
        }

        public object GetEntry(int id)
        {
            return Entries.FirstOrDefault(x => x.Entity.Id == id)!;
        }
    }
}
