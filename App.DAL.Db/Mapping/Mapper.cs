using App.DML;
using FooEntity = App.DAL.Db.Model.Foo;
using BarEntity = App.DAL.Db.Model.Bar;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Db.Mapping
{
    public static class Mapper
    {
        public static Foo FooEntityToFoo(this FooEntity fooEntity)
        {
            return new Foo
            {
                Guid = fooEntity.Guid,
                Id = fooEntity.Id,
                Description = fooEntity.Description,
                Name = fooEntity.Name
            };
        }

        public static FooEntity FooToFooEntity(this Foo foo)
        {
            return new FooEntity
            {
                Guid = foo.Guid,
                Description = foo.Description,
                Name = foo.Name
            };
        }

        public static void UpdateFooEntityByFoo(this FooEntity fooEntity, Foo foo)
        {
            fooEntity.Description = foo.Description;
            fooEntity.Name = foo.Name;
            fooEntity.Guid = foo.Guid;
        }

        public static Bar BarEntityToBar(this BarEntity barEntity)
        {
            return new Bar
            {
                Guid = barEntity.Guid,
                Id = barEntity.Id,
                Description = barEntity.Description,
                Name = barEntity.Name
            };
        }

        public static BarEntity BarToBarEntity(this Bar bar)
        {
            return new BarEntity
            {
                Guid = bar.Guid,
                Description = bar.Description,
                Name = bar.Name
            };
        }

        public static void UpdateBarEntityByBar(this BarEntity barEntity, Bar bar)
        {
            barEntity.Description = bar.Description;
            barEntity.Name = bar.Name;
            barEntity.Guid = bar.Guid;
        }

        public static Project ProjectEntityToProject(this ProjectEntity projectEntity)
        {
            return new Project(string.Empty)
            {
                Title = projectEntity.Title,
                UniqueId = projectEntity.UniqueId,
            };
        }

        public static ProjectEntity ProjectToProjectEntity(this Project project)
        {
            return new ProjectEntity
            {
                Title = project.Title,
                UniqueId = project.UniqueId
            };
        }

        public static void UpdateProjectEntityByProject(this ProjectEntity projectEntity, Project project)
        {
            projectEntity.Title = project.Title;
            projectEntity.UniqueId = project.UniqueId;
        }
    }
}
