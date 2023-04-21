using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.Factories.Base;
using App.DAL.Db;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.UnitOfWork
{
    public class ProjectsUnitOfWork : IDisposable
    {
        private readonly ProjectsDataContext _dbContext;
        
        private IBarRepository _barDbRepository;

        private IFooRepository _fooDbRepository;

        private readonly IRepositoryFactory<IFooRepository> _fooRepositoryFactory;

        private readonly IRepositoryFactory<IBarRepository> _barRepositoryFactory;

        public ProjectsUnitOfWork(
            IDbContextFactory<ProjectsDataContext> projectsDbFactory, 
            IRepositoryFactory<IFooRepository> fooRepositoryFactory,
            IRepositoryFactory<IBarRepository> barRepositoryFactory)
        {
            _dbContext = projectsDbFactory.CreateDbContext();
            _fooRepositoryFactory = fooRepositoryFactory;
            _barRepositoryFactory = barRepositoryFactory;
        }

        public IBarRepository BarRepository =>
            _barDbRepository ??= 
                _barRepositoryFactory.Create(_dbContext);

        public IFooRepository FooRepository =>
            _fooDbRepository ??=
                _fooRepositoryFactory.Create(_dbContext);

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _fooDbRepository.Dispose();
                    _barDbRepository.Dispose();
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
