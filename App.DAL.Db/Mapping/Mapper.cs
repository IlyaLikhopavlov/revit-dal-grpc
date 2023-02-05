using App.DAL.Db.Model;
using App.DML;
using Bar = App.DML.Bar;
using FooEntity = App.DAL.Db.Model.Foo;
using BarEntity = App.DAL.Db.Model.Bar;
using Foo = App.DML.Foo;
using Project = App.DML.Project;
using ProjectEntity = App.DAL.Db.Model.Project;

namespace App.DAL.Db.Mapping
{
    public static class Mapper
    {
        //private static readonly IDictionary<Type, Func<object, Element>> ConvertEntityToModel =
        //    new Dictionary<Type, Func<object, Element>>
        //    {
        //        { typeof(FooEntity), e => ((FooEntity)e).FooEntityToFoo() },
        //        { typeof(BarEntity), e => ((BarEntity)e).BarEntityToBar() },
        //        { typeof(ProjectEntity), e => ((ProjectEntity)e).ProjectEntityToProject() }
        //    };

        private static Foo FooEntityToFoo(this FooEntity fooEntity)
        {
            return new Foo
            {
                Guid = fooEntity.Guid,
                Id = fooEntity.Id,
                Description = fooEntity.Description,
                Name = fooEntity.Name
            };
        }

        private static FooEntity FooToFooEntity(this Foo foo)
        {
            return new FooEntity
            {
                Guid = foo.Guid,
                Description = foo.Description,
                Name = foo.Name
            };
        }

        private static void UpdateFooEntityByFoo(this FooEntity fooEntity, Foo foo)
        {
            fooEntity.Description = foo.Description;
            fooEntity.Name = foo.Name;
            fooEntity.Guid = foo.Guid;
        }

        private static Bar BarEntityToBar(this BarEntity barEntity)
        {
            return new Bar
            {
                Guid = barEntity.Guid,
                Id = barEntity.Id,
                Description = barEntity.Description,
                Name = barEntity.Name
            };
        }

        private static BarEntity BarToBarEntity(this Bar bar)
        {
            return new BarEntity
            {
                Guid = bar.Guid,
                Description = bar.Description,
                Name = bar.Name
            };
        }

        private static void UpdateBarEntityByBar(this BarEntity barEntity, Bar bar)
        {
            barEntity.Description = bar.Description;
            barEntity.Name = bar.Name;
            barEntity.Guid = bar.Guid;
        }

        private static Project ProjectEntityToProject(this ProjectEntity projectEntity)
        {
            return new Project(string.Empty)
            {
                Title = projectEntity.Title,
                UniqueId = projectEntity.UniqueId,
            };
        }

        private static ProjectEntity ProjectToProjectEntity(this Project project)
        {
            return new ProjectEntity
            {
                Title = project.Title,
                UniqueId = project.UniqueId
            };
        }

        private static void UpdateProjectEntityByProject(this ProjectEntity projectEntity, Project project)
        {
            projectEntity.Title = project.Title;
            projectEntity.UniqueId = project.UniqueId;
        }
    }
}
