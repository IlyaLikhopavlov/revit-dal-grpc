using Revit.Storage.Infrastructure;

namespace Revit.Storage;

public interface IExtensibleStorageService
{
    IExtensibleStorage this[string name] { get; }
}