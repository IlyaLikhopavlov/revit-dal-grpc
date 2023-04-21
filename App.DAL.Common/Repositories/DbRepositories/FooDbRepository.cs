using App.DAL.Common.Repositories.DbRepositories.Generic;
using App.DAL.Db;
using App.DAL.Db.Mapping.Abstractions;
using App.DML;
using Microsoft.EntityFrameworkCore;
using FooEntity = App.DAL.Db.Model.Foo;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class FooDbRepository : GenericDbRepository<Foo, FooEntity>, IFooRepository
    {
        public FooDbRepository(
            IEntityConverter<Foo, FooEntity> entityConverter,
            ProjectsDataContext dbContext,
            DocumentDescriptor documentDescriptor) : 
                base(entityConverter, dbContext, documentDescriptor)
        {
        }
    }
}
