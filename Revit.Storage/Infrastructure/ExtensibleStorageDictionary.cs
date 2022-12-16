using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.Storage.Infrastructure.Model;

namespace Revit.Storage.Infrastructure
{
    public class ExtensibleStorageDictionary : ExtensibleStorageCollection<Dictionary<string, string>, IDictionary<string, string>>
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

            return Storage.TryGetValue(key, out var resultString) ? resultString : null;
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
        }

        public bool RemoveEntity(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            return Storage.Remove(key);
        }

        public int GetNextId(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            if (Storage == null)
            {
                Pull();
            }

            var keys = Storage.Keys.Where(k => k.StartsWith(prefix)).ToList();
            return !keys.Any() ? 1 : keys.Max(k => GetId(k, prefix)) + 1;
        }

        public static int GetId(string keyId, string prefix) => int.Parse(keyId.Replace(prefix + "|", string.Empty));
        
        public IEnumerable<string> GetAll<T>(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            if (Storage == null)
            {
                Pull();
            }

            return Storage.Keys.Where(k => k.StartsWith(prefix)).Select(GetEntity);
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddMapField(FieldName, typeof(string), typeof(string));
        }
    }
}
