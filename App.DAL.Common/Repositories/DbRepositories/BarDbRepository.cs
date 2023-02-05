using App.DAL.Common.Repositories.DbRepositories.Generic;
using App.DAL.Db;
using App.DAL.Db.Mapping;
using App.DML;
using Microsoft.EntityFrameworkCore;
using BarEntity = App.DAL.Db.Model.Bar;

namespace App.DAL.Common.Repositories.DbRepositories
{
    public class BarDbRepository : GenericDbRepository<Bar, BarEntity>, IBarRepository
    {
        public BarDbRepository(
            IDbContextFactory<ProjectsDataContext> dbContextFactory, 
            IEntityConverter<Bar, BarEntity> entityConverter, 
            DocumentDescriptor documentDescriptor) 
            : base(dbContextFactory, entityConverter, documentDescriptor)
        {
        }
    }
}
