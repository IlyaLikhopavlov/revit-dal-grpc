using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.DataInfrastructure.Enums;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class FooRevitRepository : IFooRepository
    {
        private readonly IDataContext _context;

        public FooRevitRepository(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory,
            DocumentDescriptor documentDescriptor)
        {
            _context = dataContextFactory.New(documentDescriptor);
        }

        public IEnumerable<Foo> GetAll()
        {
            return _context.Foo;
        }

        public Foo GetById(int elementId)
        {
            return _context.Foo.Find(elementId);
        }

        public void Insert(Foo element)
        {
            _context.Foo.Add(element);
        }

        public void Delete(int elementId)
        {
            _context.Foo.Remove(elementId);
        }

        public void Update(Foo element)
        {
            var entity = _context.Foo.Entries.First(x => x.Id == element.Id);
            entity.Entity = element;
            entity.EntityState = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}
