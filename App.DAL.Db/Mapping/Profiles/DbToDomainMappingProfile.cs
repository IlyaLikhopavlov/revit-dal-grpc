using App.DML;
using AutoMapper;
using ProjectEntity = App.DAL.Db.Model.Project;
using BarEntity = App.DAL.Db.Model.Bar;
using FooEntity = App.DAL.Db.Model.Foo;

namespace App.DAL.Db.Mapping.Profiles
{
    public class DbToDomainMappingProfile : Profile
    {
        public DbToDomainMappingProfile()
        {
            CreateMap<ProjectEntity, Project>().ReverseMap();
            CreateMap<BarEntity, Bar>().ReverseMap();
            CreateMap<FooEntity, Foo>().ReverseMap();
        }
    }
}
