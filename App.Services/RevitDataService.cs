using App.CommunicationServices.Grpc;
using App.DAL.DataContext;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;

namespace App.Services
{
    public class RevitDataService
    {
        private readonly IDataContext _dataContext;

        private readonly RevitExtraDataExchangeClient _revitExtraDataExchangeClient;

        public RevitDataService(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory, 
            RevitExtraDataExchangeClient revitExtraDataExchangeClient,
            DocumentDescriptor documentDescriptor)
        {
            _revitExtraDataExchangeClient = revitExtraDataExchangeClient;
            _dataContext = dataContextFactory.New(documentDescriptor);
        }

        public async Task AllocateFoosAsync()
        {
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Foo))!;

            foreach (var id in allocated)
            {
                _dataContext?.Foo.Attach(new Foo { Id = id, Name = $@"Foo {id}", Description = @"description" });
            }

            await _dataContext?.SaveChanges()!;
        }

        public async Task AllocateBarsAsync()
        {
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Bar))!;

            foreach (var id in allocated)
            {
                _dataContext?.Bar.Attach(new Bar { Id = id, Name = $@"Bar {id}", Description = @"description" });
            }

            await _dataContext?.SaveChanges()!;
        }

        public IEnumerable<Foo> GetFoos()
        {
            return _dataContext.Foo.Entries.Select(x => x.Entity);
        }

        public IEnumerable<Bar> GetBars()
        {
            return _dataContext.Bar.Entries.Select(x => x.Entity);
        }
    }
}