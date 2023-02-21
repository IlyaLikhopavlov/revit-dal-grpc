using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DML;

namespace App.DAL.Db.Mapping.Abstractions
{
    public abstract class EntityConverter<TModel, TEntity> : IEntityConverter<TModel, TEntity>
    {
        private readonly IMapper _mapper;

        protected EntityConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public virtual TModel ConvertToModel(TEntity entity)
        {
            return _mapper.Map<TModel>(entity);
        }

        public virtual TEntity ConvertToEntity(TModel element)
        {
            return _mapper.Map<TEntity>(element);
        }

        public virtual void UpdateEntity(TModel model, ref TEntity entity)
        {
            _mapper.Map(model, entity);
        }
    }
}
