namespace App.DAL.Revit.DataContext.DataInfrastructure
{
    public interface IRevitSetBase
    {
        Type InternalEntityType { get; }

        IEnumerable<object> Entities { get; }

        object GetEntity(int id);

        object GetEntry(int id);

        Task PullRevitEntities();
    }
}
