using System;
using System.Collections.Generic;
using System.Linq;
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

        public FooDbRepository(IDbContextFactory<ProjectsDataContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public IEnumerable<Foo> GetAll()
        {
            return _dbContext.Foos.Select(x => x.FooEntityToFoo()).ToList();
        }

        public Foo GetById(int elementId)
        {
            return _dbContext.Foos.FirstOrDefault(x => x.Id == elementId)?.FooEntityToFoo();
        }

        public void Insert(Foo element)
        {
            _dbContext.Foos.Add(element.FooToFooEntity());
            Save();
        }

        public void Delete(int elementId)
        {
            var entity = _dbContext.Foos.First(x => x.Id == elementId);
            _dbContext.Foos.Remove(entity);
            Save();
        }

        public void Update(Foo element)
        {
            var entity = _dbContext.Foos.First(x => x.Id == element.Id);
            entity.UpdateFooEntityByFoo(element);
            Save();

        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose() => _dbContext?.Dispose();
    }
}
