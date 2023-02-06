using App.CommunicationServices.Grpc;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.Factories;
using App.DAL.Common.Repositories.Factories.Base;
using App.DML;
using Bimdance.Framework.Initialization;
using System.Linq;

namespace App.Services
{
    public class RevitDataService : IAsyncInitialization
    {
        private readonly IFooRepository _fooRepository;
        private readonly IBarRepository _barRepository;

        private readonly RevitExtraDataExchangeClient _revitExtraDataExchangeClient;

        public RevitDataService(
            IRepositoryFactory<IFooRepository> fooRepositoryFactory,
            IRepositoryFactory<IBarRepository> barRepositoryFactory,
            RevitExtraDataExchangeClient revitExtraDataExchangeClient)
        {
            _revitExtraDataExchangeClient = revitExtraDataExchangeClient;
            _fooRepository = fooRepositoryFactory.Create();
            _barRepository = barRepositoryFactory.Create();
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
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Bar))!;

            foreach (var id in allocated)
            {
                _barRepository.Insert(new Bar { Id = id, Name = $@"Bar {id}", Description = @"description" });
            }

            await _barRepository?.SaveAsync()!;
        }

        public IEnumerable<Foo> GetFoos()
        {
            return _fooRepository.GetAll();
        }

        public IEnumerable<Bar> GetBars()
        {
            return _barRepository.GetAll();
        }

        public IEnumerable<BaseItem> GetAllBaseEntities()
        {
            return _barRepository.GetAll().Concat(_fooRepository.GetAll().Cast<BaseItem>());
        }
    }
}