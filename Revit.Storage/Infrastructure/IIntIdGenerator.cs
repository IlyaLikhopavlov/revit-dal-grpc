namespace Revit.Storage.Infrastructure;

public interface IIntIdGenerator : IExtensibleStorage
{
    int GetNewId();

    bool ReleaseId(int id);

    void Save();
}