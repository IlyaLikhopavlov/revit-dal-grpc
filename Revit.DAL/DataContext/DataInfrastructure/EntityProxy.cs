using App.DAL.DataContext.DataInfrastructure.Enums;
using Revit.DML;

namespace App.DAL.DataContext.DataInfrastructure
{
    public class EntityProxy<TModelElement> : IEntityProxy
        where TModelElement : Element
    {
        public EntityProxy(TModelElement entity, int id, EntityState entityState = EntityState.Unchanged)
        {
            Id = id;
            Entity = entity;
            EntityState = entityState;
        }

        public EntityProxy() : this(null, -1)
        {
        }

        public int Id { get; internal set; }

        public TModelElement Entity { get; set; }

        public EntityState EntityState { get; set; }
    }
}
