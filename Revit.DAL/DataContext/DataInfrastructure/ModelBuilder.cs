using Revit.DAL.DataContext.DataInfrastructure.Configurations;
using Revit.DAL.Utils;

namespace Revit.DAL.DataContext.DataInfrastructure
{
    public class ModelBuilder
    {
        private readonly IEnumerable<IRevitSet> _revitSets;

        public ModelBuilder(IEnumerable<IRevitSet> revitSets)
        {
            _revitSets = revitSets;
        }

        public ConfigurationsRepository ConfigurationsRepository { get; } = new();

        private void PullEntities() => _revitSets.ForEach(revitSet => revitSet.PullRevitEntities());

        private void ResolveRelations()
        {
            foreach (var revitSet in _revitSets)
            {
                var propertyRelations = 
                    ConfigurationsRepository.GetPropertyRelations(revitSet.InternalEntityType).ToArray();

                if (revitSet.EntityPairs == null)
                {
                    continue;
                }

                foreach (var revitSetEntityPair in revitSet.EntityPairs)
                {
                    foreach (var propertyRelation in propertyRelations)
                    {
                        propertyRelation.Resolve(revitSetEntityPair.Source, revitSetEntityPair.Target);
                    }
                }
            }
        }

        public void Build()
        {
            PullEntities();
            ResolveRelations();
        }
    }
}
