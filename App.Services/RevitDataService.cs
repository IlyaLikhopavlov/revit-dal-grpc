using App.Catalog.Db.Model;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.Factories.Base;
using App.DAL.Common.Services.Catalog;
using App.DML;
using Bimdance.Framework.Initialization;
using System;
using App.Catalog.Db.Model.Enums;

namespace App.Services
{
    public class RevitDataService : IAsyncInitialization
    {
        private readonly IFooRepository _fooRepository;
        private readonly IBarRepository _barRepository;
        private readonly ICatalogService _catalogService;

        private readonly RevitExtraDataExchangeClient _revitExtraDataExchangeClient;


        public RevitDataService(
            IRepositoryFactory<IFooRepository> fooRepositoryFactory,
            IRepositoryFactory<IBarRepository> barRepositoryFactory,
            RevitExtraDataExchangeClient revitExtraDataExchangeClient, 
            IDocumentDescriptorServiceScopeFactory scopeFactory)
        {
            _revitExtraDataExchangeClient = revitExtraDataExchangeClient;
            _catalogService = scopeFactory.GetScopedService<ICatalogService>();
            _fooRepository = fooRepositoryFactory.Create();
            _barRepository = barRepositoryFactory.Create();
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            if (_fooRepository is IAsyncInitialization fooRepository)
            {
                await fooRepository.Initialization;
            }

            if (_barRepository is IAsyncInitialization barRepository)
            {
                await barRepository.Initialization;
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
            var foos = _fooRepository.GetAll();
            var bars = _barRepository.GetAll();

            if (foos == null || bars == null)
            {
                return new List<BaseItem>();
            }

            return foos.Concat(bars.Cast<BaseItem>());
        }

        public async Task AddNewBarAsync()
        {
            var bar = new Bar
            {
                Name = "BarNew",
                Description = "Weee",
                Guid = Guid.NewGuid()
            };

            var category = new Category
            {
                Name = @"Third",
                Description = @"Third"
            };

            _barRepository.Insert(bar, category);

            await _barRepository.SaveAsync();
        }

        public async Task RemoveEntityAsync(int id)
        {
            if (_fooRepository.Contains(id))
            {
                _fooRepository.Remove(id);
                await _fooRepository.SaveAsync();
                return;
            }
            
            if (_barRepository.Contains(id))
            {
                _barRepository.Remove(id);
                await _barRepository.SaveAsync();
                return;
            }
            
            throw new InvalidOperationException($"Element with ID={id} wasn't found.");
        }

        public async Task AddCatalogEntry()
        {
            var fooCatalog = new FooCatalog
            {
                IdGuid = Guid.Parse("573E1D97-4C3B-4841-9BB6-28BF8CE3F07B"),
                Description = "Some new entity",
                ModelNumber = "12345679",
                PartNumber = "HHHHKKKK",
                Version = 1
            };

            fooCatalog.FooCatalogChannels.Add(new FooCatalogChannel
            {
                Channel = new Channel
                {
                    Name = "ChannelN",
                    Type = ChannelTypeEnum.Temperature
                }
            });

            await _catalogService.WriteCatalogRecordAsync(fooCatalog);
        }
    }
}