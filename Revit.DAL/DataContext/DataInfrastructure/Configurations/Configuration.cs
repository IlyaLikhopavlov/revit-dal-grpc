using Revit.DML;

namespace Revit.DAL.DataContext.DataInfrastructure.Configurations
{
    public class Configuration<TRevitEntity, TEntity> : IConfiguration
        where TEntity : Element
        where TRevitEntity : Autodesk.Revit.DB.Element
    {
        private readonly IList<PropertyRelation<TRevitEntity, TEntity>> _propertyRelations;

        public Configuration(IList<PropertyRelation<TRevitEntity, TEntity>> propertyRelations)
        {
            _propertyRelations = propertyRelations;
        }

        public IReadOnlyCollection<IPropertyRelation> PropertyRelations => _propertyRelations.ToArray();

        public Type Type => typeof(TEntity);
    }
}
