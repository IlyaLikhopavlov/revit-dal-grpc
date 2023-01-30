using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Db;
using App.DAL.Db.Mapping;
using App.DML;
using Microsoft.EntityFrameworkCore;
using FooEntity = App.DAL.Db.Model.Foo;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class FooDbRepository : IFooRepository
    {
        private readonly ProjectsDataContext _dbContext;

        private readonly DocumentDescriptor _documentDescriptor;

        public FooDbRepository(IDbContextFactory<ProjectsDataContext> dbContextFactory, 
            DocumentDescriptor documentDescriptor)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _documentDescriptor = documentDescriptor;
        }

        public IEnumerable<Foo> GetAll()
        {
            return 
                _dbContext.Foos
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .Select(x => x.FooEntityToFoo())
                    .ToList();
        }

        public Foo GetById(int elementId)
        {
            return 
                _dbContext.Foos
                    .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                    .First(x => x.Id == elementId)
                    .FooEntityToFoo();
        }

        public void Insert(Foo element)
        {
            var project =_dbContext.Projects.First(x => x.UniqueId == _documentDescriptor.Id);

            var entity = element.FooToFooEntity();
            entity.ProjectId = project.Id;

            _dbContext.Foos.Add(entity);
            Save();
        }

        public void Delete(int elementId)
        {
            var entity = _dbContext.Foos
                .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                .First(x => x.Id == elementId);
            _dbContext.Foos.Remove(entity);
            Save();
        }

        public void Update(Foo element)
        {
            var entity = _dbContext.Foos
                .Where(x => x.Project.UniqueId == _documentDescriptor.Id)
                .First(x => x.Id == element.Id);
            entity.UpdateFooEntityByFoo(element);
            _dbContext.Foos.Update(entity);
            Save();

        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
