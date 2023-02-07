using App.DAL.Db.Mapping.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DML;
using AutoMapper;
using BarEntity = App.DAL.Db.Model.Bar;

namespace App.DAL.Db.Mapping
{
    public class BarEntityConverter : IEntityConverter<Bar, BarEntity>
    {
        private readonly IMapper _mapper;

        public BarEntityConverter(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public Bar ConvertToModel(BarEntity entity)
        {
            return _mapper.Map<Bar>(entity);
        }

        public BarEntity ConvertToEntity(Bar element)
        {
            return _mapper.Map<BarEntity>(element);
        }

        public void UpdateEntity(Bar model, ref BarEntity entity)
        {
            _mapper.Map(model, entity);
        }
    }
}
