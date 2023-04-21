using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Db;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.UnitOfWork
{
    public class ProjectsUnitOfWork : IDisposable
    {
        private readonly ProjectsDataContext _dbContext;
        
        private readonly IDocumentDescriptorServiceScopeFactory _descriptorServiceScopeFactory;

        private BarDbRepository _barDbRepository;

        private FooDbRepository _fooDbRepository;

        public ProjectsUnitOfWork(
            IDbContextFactory<ProjectsDataContext> projectsDbFactory, 
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory)
        {
            _dbContext = projectsDbFactory.CreateDbContext();
            _descriptorServiceScopeFactory = documentDescriptorServiceScopeFactory;
        }

        public BarDbRepository BarDbRepository =>
            _barDbRepository ??= 
                (BarDbRepository)_descriptorServiceScopeFactory.GetScopedService(typeof(BarDbRepository), _dbContext);

        public FooDbRepository FooDbRepository =>
            _fooDbRepository ??=
                (FooDbRepository)_descriptorServiceScopeFactory.GetScopedService(typeof(FooDbRepository), _dbContext);

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
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
