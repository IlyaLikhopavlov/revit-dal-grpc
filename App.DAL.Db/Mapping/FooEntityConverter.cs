using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using AutoMapper;
using FooEntity = App.DAL.Db.Model.Foo;

namespace App.DAL.Db.Mapping
{
    public class FooEntityConverter : IEntityConverter<Foo, FooEntity>
    {
        private readonly IMapper _mapper;

        public FooEntityConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Foo ConvertToModel(FooEntity entity)
        {
            return _mapper.Map<Foo>(entity);
        }

        public FooEntity ConvertToEntity(Foo element)
        {
            return _mapper.Map<FooEntity>(element);
        }

        public void UpdateEntity(Foo model, ref FooEntity entity)
        {
            _mapper.Map(model, entity);
        }
    }
}
