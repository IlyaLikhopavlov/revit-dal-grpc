using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Db.Model;
using App.DML;

namespace App.DAL.Db.Mapping
{
    public interface IEntityConverter<TModel, TEntity>
        where TModel : Element
        where TEntity : BaseEntity
    {
        TModel ConvertToModel(TEntity entity);

        TEntity ConvertToEntity(TModel entity);

        void UpdateEntity(TModel model, ref TEntity entity);
    }
}
