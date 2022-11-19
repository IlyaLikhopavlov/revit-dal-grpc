using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Utils;

namespace Revit.DAL.Storage.Infrastructure
{
    public abstract class ExtensibleStorageCollection<TTarget, TBase>
        where TTarget : class, TBase, new()
    {
        public const string TransactionNamePrefix = @"Save dictionary";

        protected readonly Guid SchemaGuid;

        protected readonly Schema Schema;

        protected readonly string Name;

        protected readonly string FieldName;

        protected readonly Element Element;

        protected TBase Storage;

        protected readonly Guid Guid;

        protected ExtensibleStorageCollection(string guid, string name, string fieldName, Element element)
        {
            SchemaGuid = new Guid(guid);
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($@"Schema name {name} or field name {fieldName} isn't applicable.");
            }

            Name = name;
            FieldName = fieldName;
            Element = element ?? throw new ArgumentNullException();
            Schema = GetSchema();
            Guid = Guid.NewGuid();
        }

        protected abstract void AddField(SchemaBuilder schemaBuilder);

        protected Schema GetSchema()
        {
            var schema = Schema.ListSchemas().FirstOrDefault(x => x.GUID == SchemaGuid);

            if (schema != null)
            {
                return schema;
            }

            var schemaBuilder = new SchemaBuilder(SchemaGuid);

            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
            AddField(schemaBuilder);
            schemaBuilder.SetSchemaName(Name);

            return schemaBuilder.Finish();
        }

        protected virtual void Pull()
        {
            Element.Document.ExecuteTransaction(() =>
            {
                var entity = Element.GetEntity(Schema);

                if (!entity.IsValid())
                {
                    entity = new Entity(Schema);
                }

                Storage = entity.Get<TBase>(Schema.GetField(FieldName)) ?? new TTarget();
            }, $"{TransactionNamePrefix} {Name}");
        }

        public virtual void Save()
        {
            Element.Document.ExecuteTransaction(() =>
            {
                var entity = Element.GetEntity(Schema);

                if (!entity.IsValid())
                {
                    entity = new Entity(Schema);
                }

                entity.Set(Schema.GetField(FieldName), Storage ?? new TTarget());
                Element.SetEntity(entity);
            }, $"{TransactionNamePrefix} {Name}");
        }
    }
}
