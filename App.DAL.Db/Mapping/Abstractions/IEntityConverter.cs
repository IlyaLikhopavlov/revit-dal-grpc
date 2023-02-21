using App.DAL.Db.Model;
using App.DML;

namespace App.DAL.Db.Mapping.Abstractions
{
    public interface IEntityConverter<TModel, TEntity>
    {
        TModel ConvertToModel(TEntity entity);

        TEntity ConvertToEntity(TModel element);

        void UpdateEntity(TModel model, ref TEntity entity);
    }
}
