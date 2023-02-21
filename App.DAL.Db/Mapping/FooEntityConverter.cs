using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using AutoMapper;
using FooEntity = App.DAL.Db.Model.Foo;

namespace App.DAL.Db.Mapping
{
    public class FooEntityConverter : EntityConverter<Foo, FooEntity>
    {
        public FooEntityConverter(IMapper mapper) : base(mapper)
        {
        }
    }
}
