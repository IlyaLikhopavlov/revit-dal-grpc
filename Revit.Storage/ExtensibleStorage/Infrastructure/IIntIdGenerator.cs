namespace Revit.Storage.ExtensibleStorage.Infrastructure;

public interface IIntIdGenerator : IExtensibleStorage
{
    int GetNewId();

    bool ReleaseId(int id);

    void Save();
}