using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using AutoMapper;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Db.Mapping
{
    public class ProjectEntityConverter : EntityConverter<Project, ProjectEntity>
    {
        public ProjectEntityConverter(IMapper mapper) : base(mapper)
        {
        }
    }
}
