namespace Revit.DAL.Storage.Infrastructure;

public interface IIntIdGenerator
{
    int GetNewId();

    bool ReleaseId(int id);

    void Save();
}