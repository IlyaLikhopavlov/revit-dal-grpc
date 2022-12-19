using Revit.Storage.ExtensibleStorage.Schemas;
using Autodesk.Revit.DB;

namespace Revit.Storage.ExtensibleStorage.Infrastructure
{
    public interface IExtensibleStorageDataSchema : IExtensibleStorage
    {
        DataSchema GetEntity(Element element);

        void AddEntity(Element element, DataSchema value);

        void UpdateEntity(Element element, DataSchema value);
    }
}
