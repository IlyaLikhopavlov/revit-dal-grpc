using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Infrastructure;

namespace Revit.Storage.ExtensibleStorage;

public interface IExtensibleStorageService
{
    IExtensibleStorage this[string entityName] { get; }

    IExtensibleStorageDataSchema GetDataSchemaStorage(DomainModelTypesEnum entityType);

    IExtensibleStorageDictionary GetDictionaryStorage(string entityName);

    IIntIdGenerator GetIdGenerator(string entityName);
}