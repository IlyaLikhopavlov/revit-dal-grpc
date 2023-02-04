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

            var foo = 
                new Foo
                {
                    Description = "Foo1 description from DB",
                    Guid = new Guid("B2B40507-0973-4F62-8C5F-26D1F57CDB3E"),
                    Name = "Foo1",
                    Project = project
                };

            var bar = 
                new Bar
                {
                    Description = "Bar1 description from DB",
                    Guid = new Guid("87548BE0-6552-4016-9F86-89EADE37390F"),
                    Name = "Bar1",
                    Project = project
                };

            context.Bars.Add(bar);
            context.Foos.Add(foo);

            context.SaveChanges();
        }
    }
}
