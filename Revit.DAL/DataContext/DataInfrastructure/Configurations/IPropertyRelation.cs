namespace Revit.DAL.DataContext.DataInfrastructure.Configurations
{
    public interface IPropertyRelation
    {
        Type SourceType { get; }

        Type TargetType { get; }

        Func<object, object> Resolver { get; set; }

        void Resolve(object source, object? target);
    }
}
