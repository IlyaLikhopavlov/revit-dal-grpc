using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Db;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.UnitOfWork
{
    public class ProjectsUnitOfWork : IDisposable
    {
        private readonly ProjectsDataContext _dbContext;
        
        private readonly IServiceProvider _serviceProvider;


        public ProjectsUnitOfWork(
            IDbContextFactory<ProjectsDataContext> projectsDbFactory, 
            //IDocumentDescriptorServiceScopeFactory serviceProvider, 
            DocumentDescriptor documentDescriptor)
        {
            _dbContext = projectsDbFactory.CreateDbContext();
            //_serviceProvider = serviceProvider;
        }

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
