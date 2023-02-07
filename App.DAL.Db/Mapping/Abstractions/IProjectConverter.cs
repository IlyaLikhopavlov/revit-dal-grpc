using App.DML;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Db.Mapping.Abstractions
{
    public interface IProjectConverter
    {
        Project ConvertToModel(ProjectEntity entity);

        ProjectEntity ConvertToEntity(Project element);

        void UpdateEntity(Project model, ref ProjectEntity entity);
    }
}
