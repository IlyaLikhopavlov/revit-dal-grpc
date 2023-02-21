using App.DAL.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Db
{
    public class ProjectsDbInitializer
    {
        private readonly IDbContextFactory<ProjectsDataContext> _dbContextFactory;

        public ProjectsDbInitializer(IDbContextFactory<ProjectsDataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void InitDataBase()
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Database.Migrate();

            if (context.Projects.Any())
            {
                return;
            }

            var project = 
                new Project
                {
                    Title = "Project 1",
                    UniqueId = "F66BDFAD-0FB7-45EE-937B-ACC384033DC4",
                };

            context.Projects.Add(project);

            var specialCategory = 
                new Category
                {
                    Name = "Special"
                };

            var usualCategory =
                new Category
                {
                    Name = "Usual"
                };

            context.Categories.Add(specialCategory);
            context.Categories.Add(usualCategory);

            var foo1 = 
                new Foo
                {
                    Description = "Foo1 description from DB",
                    Guid = new Guid("B2B40507-0973-4F62-8C5F-26D1F57CDB3E"),
                    Name = "Foo1",
                    Project = project
                };

            var foo2 =
                new Foo
                {
                    Description = "Foo2 description from DB",
                    Guid = new Guid("7D7A0EA5-FF45-4288-BAF8-FB85D2B9EFC4"),
                    Name = "Foo2",
                    Project = project
                };

            var bar1 = 
                new Bar
                {
                    Description = "Bar1 description from DB",
                    Guid = new Guid("87548BE0-6552-4016-9F86-89EADE37390F"),
                    Name = "Bar1",
                    Category = specialCategory,
                    Project = project
                };

            var bar2 =
                new Bar
                {
                    Description = "Bar2 description from DB",
                    Guid = new Guid("63282292-9329-44EA-A15B-18DA7BFE364B"),
                    Name = "Bar2",
                    Category = usualCategory,
                    Project = project
                };

            context.Bars.Add(bar1);
            context.Foos.Add(foo1);
            context.Bars.Add(bar2);
            context.Foos.Add(foo2);

            context.SaveChanges();
        }
    }
}
