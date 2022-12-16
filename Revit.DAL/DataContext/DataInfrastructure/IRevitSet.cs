namespace Revit.DAL.DataContext.DataInfrastructure
{
    public interface IRevitSet
    {
        Type RevitEntityType { get; }

        Type InternalEntityType { get; }

        IEnumerable<object> Entities { get; }

        object GetEntity(int id);

        object GetEntry(int id);

        void PullRevitEntities();
    }
}
