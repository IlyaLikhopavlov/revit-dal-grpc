using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Exceptions;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure
{
    public class ExtensibleStorage<T> : IExtensibleStorage where T : class, new()
    {
        private readonly Guid _schemaGuid;
        private readonly string _schemaName;

        private readonly Schema _schema;

        public ExtensibleStorage(SchemaDescriptor schemaDescriptor)
        {
            _schemaGuid = schemaDescriptor.Guid;
            _schemaName = schemaDescriptor.EntityName;
            _schema = GetSchema();
        }

        private Schema GetSchema()
        {
            var schema = Schema.ListSchemas().FirstOrDefault(x => x.GUID == _schemaGuid);

            if (schema != null)
            {
                return schema;
            }

            var schemaBuilder = new SchemaBuilder(_schemaGuid);
            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                ExtensibleStorageUtils.AssertExtensibleStorageCompatible(propertyInfo.PropertyType);

                schemaBuilder.AddSimpleField(propertyInfo.Name, propertyInfo.PropertyType);
            }

            schemaBuilder.SetSchemaName(_schemaName);
            return schemaBuilder.Finish();
        }

        public void AddEntity(Element element, T value)
        {
            var entity = new Entity(_schema);

            UpdateEntity(element, entity, value);
        }

        public T GetEntity(Element element)
        {
            var entity = element.GetEntity(_schema);

            var result = new T();

            if (!entity.IsValid())
            {
                return result;
            }

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var entityFieldValue =
                    ExtensibleStorageUtils.GetEntityFieldValue(entity, propertyInfo.PropertyType, propertyInfo.Name);

                propertyInfo.SetValue(result, entityFieldValue, null);
            }

            return result;
        }

        private static void UpdateEntity(Element element, Entity entity, T value)
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(value, null);

                if (propertyValue == null)
                {
                    continue;
                }

                var setMethodInfo = entity.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.IsGenericMethod && x.Name == nameof(entity.Set))
                    .FirstOrDefault(x =>
                    {
                        var parameters = x.GetParameters();
                        return parameters.Length == ExtensibleStorageUtils.SetMethodParametersCount &&
                               parameters.Any(p => p.Name == ExtensibleStorageUtils.FieldNameParameterName && p.ParameterType == typeof(string));
                    });

                var setGenericInfo = setMethodInfo?.MakeGenericMethod(propertyInfo.PropertyType)
                                     ?? throw new RevitDataAccessException($"Can't get method {nameof(entity.Set)} for {nameof(Entity)}");

                setGenericInfo.Invoke(entity, new[] { propertyInfo.Name, propertyValue });
            }

            element.SetEntity(entity);
        }

        public void UpdateEntity(Element element, T value)
        {
            var entity = element.GetEntity(_schema);

            if (!entity.IsValid())
            {
                entity = new Entity(_schema);
            }

            UpdateEntity(element, entity, value);
        }

        public Type Type => GetType();
    }
}
