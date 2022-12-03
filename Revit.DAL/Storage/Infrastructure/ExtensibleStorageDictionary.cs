using System.Text.Json;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure
{
    public class ExtensibleStorageDictionary : ExtensibleStorageCollection<Dictionary<string, string>, IDictionary<string, string>>
    {
        protected JsonSerializerOptions JsonSerializerOptions = new();

        public ExtensibleStorageDictionary(SchemaDescriptor schemaDescriptor)
            : base(
                schemaDescriptor.Guid.ToString(), 
                schemaDescriptor.EntityName,
                schemaDescriptor.FieldName,
                schemaDescriptor.TargetElement)
        {
        }

        public T GetEntity<T>(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            return Storage.TryGetValue(key, out var resultString)
                    ? JsonSerializer.Deserialize<T>(resultString, JsonSerializerOptions)
                    : default;
        }

        public void AddEntity<T>(string key, T entity)
        {
            if (Storage == null)
            {
                Pull();
            }

            if (!Storage.ContainsKey(key))
            {
                Storage.Add(key, JsonSerializer.Serialize(entity, JsonSerializerOptions));
            }
        }

        public void UpdateEntity<T>(string key, T entity)
        {
            if (Storage == null)
            {
                Pull();
            }

            if (!Storage.ContainsKey(key))
            {
                return;
            }

            Storage[key] = JsonSerializer.Serialize(entity, JsonSerializerOptions);
        }

        public bool RemoveEntity(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            return Storage.Remove(key);
        }

        //todo is it really require?
        public string GetEntityJson(string key)
        {
            if (Storage == null)
            {
                Pull();
            }

            return Storage.TryGetValue(key, out var resultString)
                ? resultString
                : default;
        }

        
        public void AddEntityJson(string key, string entityJson)
        {
            if (Storage == null)
            {
                Pull();
            }

            if (!Storage.ContainsKey(key))
            {
                Storage.Add(key, entityJson);
            }
        }

        public bool Contains<T>(T t, string prefix = null, IEqualityComparer<T> comparer = null)
        {
            if (Storage == null)
            {
                Pull();
            }

            var keys = string.IsNullOrEmpty(prefix)
                ? Storage.Keys
                : Storage.Keys.Where(k => k.StartsWith(prefix));

            return keys.Any(key =>
            {
                var entity = GetEntity<T>(key);
                return comparer?.Equals(entity, t) ?? entity.Equals(t);
            });
        }

        //todo that logic has to belong to derived class. Why here?
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

        //todo also strange place for that logic
        public IEnumerable<T> GetAll<T>(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            if (Storage == null)
            {
                Pull();
            }

            return Storage.Keys.Where(k => k.StartsWith(prefix)).Select(GetEntity<T>);
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddMapField(FieldName, typeof(string), typeof(string));
        }
    }
}
