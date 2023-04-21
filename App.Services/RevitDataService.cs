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
using App.Catalog.Db;
using App.DAL.Common.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Services
{
    public class RevitDataService : IAsyncInitialization
    {
        private readonly ICatalogService _catalogService;
        private readonly RevitCatalogStorage _revitCatalogStorage;
        private readonly IServiceProvider _serviceProvider;

        private readonly RevitExtraDataExchangeClient _revitExtraDataExchangeClient;


        public RevitDataService(
            IServiceProvider serviceProvider,
            RevitExtraDataExchangeClient revitExtraDataExchangeClient,
            IDocumentDescriptorServiceScopeFactory scopeFactory)
        {
            _revitExtraDataExchangeClient = revitExtraDataExchangeClient;
            _revitCatalogStorage = scopeFactory.GetScopedService<RevitCatalogStorage>();
            _catalogService = scopeFactory.GetScopedService<ICatalogService>();
            _serviceProvider = serviceProvider;
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            //if (_fooRepository is IAsyncInitialization fooRepository)
            //{
            //    await fooRepository.Initialization;
            //}

            //if (_barRepository is IAsyncInitialization barRepository)
            //{
            //    await barRepository.Initialization;
            //}
        }

        public Task Initialization { get; }

        public async Task AllocateFoosAsync()
        {
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Foo))!;
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();

            foreach (var id in allocated)
            {
                uow.FooRepository.Insert(new Foo { Id = id, Name = $@"Foo {id}", Description = @"description" });
            }

            await uow.FooRepository?.SaveAsync()!;
        }

        public async Task AllocateBarsAsync()
        {
            var allocated = await _revitExtraDataExchangeClient?.Allocate(typeof(Bar))!;
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();

            foreach (var id in allocated)
            {
                uow.BarRepository.Insert(new Bar { Id = id, Name = $@"Bar {id}", Description = @"description" });
            }

            await uow.BarRepository?.SaveAsync()!;
        }

        public IEnumerable<Foo> GetFoos()
        {
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();
            return uow.FooRepository.GetAll();
        }

        public IEnumerable<Bar> GetBars()
        {
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();
            return uow.BarRepository.GetAll();
        }

        public IEnumerable<BaseItem> GetAllBaseEntities()
        {
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();
            var foos = uow.FooRepository.GetAll();
            var bars = uow.BarRepository.GetAll();

            if (foos == null || bars == null)
            {
                return new List<BaseItem>();
            }

            return foos.Concat(bars.Cast<BaseItem>());
        }

        //public async Task AddNewBarAsync()
        //{
        //    var bar = new Bar
        //    {
        //        Name = "BarNew",
        //        Description = "Weee",
        //        Guid = Guid.NewGuid()
        //    };

        //    var category = new Category
        //    {
        //        Name = @"Third",
        //        Description = @"Third"
        //    };

        //    _barRepository.Insert(bar, category);

        //    await _barRepository.SaveAsync();
        //}

        public async Task RemoveEntityAsync(int id)
        {
            using var uow = _serviceProvider.GetRequiredService<ProjectsUnitOfWork>();
            if (uow.FooRepository.Contains(id))
            {
                uow.FooRepository.Remove(id);
                await uow.FooRepository.SaveAsync();
                return;
            }
            
            if (uow.BarRepository.Contains(id))
            {
                uow.BarRepository.Remove(id);
                await uow.BarRepository.SaveAsync();
                return;
            }
            
            throw new InvalidOperationException($"Element with ID={id} wasn't found.");
        }

        public async Task AddCatalogEntry()
        {

            //var fooCatalog = await _catalogService.ReadCatalogRecordAsync<FooCatalog>(
            //    Guid.Parse("0117C24B-B01E-4D07-9FCC-654BA92E50CC"),
            //    p => p
            //        .Include(c => c.FooCatalogChannels)
            //        .ThenInclude(c => c.Channel));

            //var barCatalog = await _catalogService.ReadCatalogRecordAsync<BarCatalog>(
            //    Guid.Parse("5B480CB6-7CE1-4BE1-BA42-854111F17244"),
            //    p => p
            //        .Include(c => c.BarCatalogChannels)
            //        .ThenInclude(c => c.Channel));

            var result = await _catalogService.CompareAllAsync();

            await _catalogService.SynchronizeAsync(result);
        }
    }
}