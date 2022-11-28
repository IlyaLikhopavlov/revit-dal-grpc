using Autodesk.Revit.DB;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage;

public interface IExtensibleStorageService
{
    IExtensibleStorage this[string name] { get; }

    (string data, SchemaDescriptor descriptor) GetDataFromElement(Element element);

    IEnumerable<(string data, SchemaDescriptor descriptor)> GetProjectData();
}