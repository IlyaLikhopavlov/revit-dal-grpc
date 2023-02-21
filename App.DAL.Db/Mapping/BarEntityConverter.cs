using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using AutoMapper;
using BarEntity = App.DAL.Db.Model.Bar;

namespace App.DAL.Db.Mapping
{
    public class BarEntityConverter : EntityConverter<Bar, BarEntity>
    {
        public BarEntityConverter(IMapper mapper) : base(mapper)
        {
        }
    }
}
