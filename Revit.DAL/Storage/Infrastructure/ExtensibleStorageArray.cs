using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit.DAL.Storage.Infrastructure
{
    public class ExtensibleStorageArray<T> : ExtensibleStorageCollection<List<T>, IList<T>>
    {
        public ExtensibleStorageArray(string guid, string name, string fieldName, Element element) : base(guid, name, fieldName, element)
        {
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddArrayField(FieldName, typeof(T));
        }
    }
}
