using App.DAL.DataContext.DataInfrastructure;
using App.DAL.DataContext.DataInfrastructure.Enums;
using App.DML;

namespace App.DAL.DataContext
{
    public class BaseEntityProxy
    {
        private readonly IEntityProxy _entityProxy;

        public BaseEntityProxy(IEntityProxy entityProxy)
        {
            _entityProxy = entityProxy;
        }

        public BaseItem BaseItem =>
            _entityProxy switch
            {
                EntityProxy<Foo> fooProxy => fooProxy.Entity,
                EntityProxy<Bar> barProxy => barProxy.Entity,
                _ => null
            };

        public int Id => _entityProxy.Id;

        public EntityState EntityState
        {
            set => _entityProxy.EntityState = value;
        }
    }
}