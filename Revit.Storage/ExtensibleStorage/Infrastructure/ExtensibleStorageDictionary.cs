using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.Storage.ExtensibleStorage.Infrastructure.Model;

namespace Revit.Storage.ExtensibleStorage.Infrastructure
{
    public class ExtensibleStorageDictionary : 
        ExtensibleStorageCollection<Dictionary<string, string>, IDictionary<string, string>>,
        IExtensibleStorageDictionary
    {
        public ExtensibleStorageDictionary(SchemaDescriptor schemaDescriptor)
            : base(
                schemaDescriptor.SchemaInfo.Guid.ToString(),
                schemaDescriptor.SchemaInfo.EntityName,
                schemaDescriptor.SchemaInfo.FieldName,
                schemaDescriptor.SchemaInfo.TargetElement)
        {
        }

        public string GetEntity(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            return Storage.TryGetValue(key, out var resultString) 
                ? resultString 
                : throw new ArgumentException($"Dictionary element with key {key} wasn't founded.");
        }

        public void AddEntity(string key, string entity)
        {
            if (Storage == null)
            {
                Pull();
            }

            if (!Storage.ContainsKey(key))
            {
                Storage.Add(key, entity);
            }
            
            Save();
        }

        public void UpdateEntity(string key, string entity)
        {
            if (Storage == null)
            {
                Pull();
            }

            if (!Storage.ContainsKey(key))
            {
                return;
            }

            Storage[key] = entity;

            Save();
        }

        public bool RemoveEntity(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            var result = Storage.Remove(key);
            Save();

            return result;
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Storage == null)
            {
                Pull();
            }

            return Storage.Keys.Any(x => x == key);
        }

        public IDictionary<string, string> GetAll()
        {
            if (Storage == null)
            {
                Pull();
            }

            return new Dictionary<string, string>(Storage);
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddMapField(FieldName, typeof(string), typeof(string));
        }
    }
}
