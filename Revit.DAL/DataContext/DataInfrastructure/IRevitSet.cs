namespace App.DAL.DataContext.DataInfrastructure
{
    public interface IRevitSet
    {
        Type InternalEntityType { get; }

        IEnumerable<object> Entities { get; }

        object GetEntity(int id);

        object GetEntry(int id);

        Task PullRevitEntities();
    }
}
