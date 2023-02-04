using App.CommunicationServices.Grpc;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.Factories;
using App.DML;
using Bimdance.Framework.Initialization;

namespace App.Services
{
    public class RevitDataService : IAsyncInitialization
    {
        private readonly IFooRepository _fooRepository; 

        private readonly RevitExtraDataExchangeClient _revitExtraDataExchangeClient;

        public RevitDataService(
            IFooRepositoryFactory fooRepositoryFactory, 
            RevitExtraDataExchangeClient revitExtraDataExchangeClient)
        {
            _revitExtraDataExchangeClient = revitExtraDataExchangeClient;
            _fooRepository = fooRepositoryFactory.Create();
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            if (_fooRepository is IAsyncInitialization repository)
            {
                await repository.Initialization;
            }
        }

        public Task Initialization { get; }

        public async Task AllocateFoosAsync()
        {
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Foo))!;

            foreach (var id in allocated)
            {
                _fooRepository.Insert(new Foo { Id = id, Name = $@"Foo {id}", Description = @"description" });
            }

            await _fooRepository?.SaveAsync()!;
        }

        public async Task AllocateBarsAsync()
        {
            //var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Bar))!;

            //foreach (var id in allocated)
            //{
            //    _dataContext?.Bar.Attach(new Bar { Id = id, Name = $@"Bar {id}", Description = @"description" });
            //}

            //await _dataContext?.SaveChanges()!;
        }

        public IEnumerable<Foo> GetFoos()
        {
            //var result = _dataContext.Foo.Entries.Select(x => x.Entity).ToList();
            return _fooRepository.GetAll();
            //return result;
        }

        public IEnumerable<Bar> GetBars()
        {
            //var result = _dataContext.Bar.Entries.Select(x => x.Entity);
            //return result;
            return Array.Empty<Bar>();
        }

        public IEnumerable<BaseItem> GetAllBaseEntities()
        {
            //return _dataContext.GetBaseEntityProxies().Select(x => x.BaseItem);
            return Array.Empty<Bar>();
        }
    }
}