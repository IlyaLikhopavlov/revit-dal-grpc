namespace Revit.DAL.DataContext.DataInfrastructure.Configurations
{
    public class ConfigurationsRepository
    {
        private readonly IList<IConfiguration> _configurations = 
            new List<IConfiguration>();

        public void Add(IConfiguration configuration)
        {
            _configurations.Add(configuration);
        }
        
        public IEnumerable<IPropertyRelation> GetPropertyRelations(Type entityType) =>
            _configurations.Where(x => x.Type == entityType).SelectMany(x => x.PropertyRelations);

    }
}
