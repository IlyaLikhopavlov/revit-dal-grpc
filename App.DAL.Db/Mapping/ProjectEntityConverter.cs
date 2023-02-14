using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using AutoMapper;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Db.Mapping
{
    public class ProjectEntityConverter : IProjectConverter
    {
        private readonly IMapper _mapper;

        public ProjectEntityConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Project ConvertToModel(ProjectEntity entity)
        {
            return _mapper.Map<Project>(entity);
        }

        public ProjectEntity ConvertToEntity(Project element)
        {
            return _mapper.Map<ProjectEntity>(element);
        }

        public void UpdateEntity(Project model, ref ProjectEntity entity)
        {
            _mapper.Map(model, entity);
        }
    }
}
