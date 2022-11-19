namespace Revit.DAL.DataContext.DataInfrastructure.Configurations
{
    public interface IConfiguration
    {
        IReadOnlyCollection<IPropertyRelation> PropertyRelations { get; }

        Type Type { get; }
    }
}
