
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.DataContext;
using Revit.DAL.DataContext.DataInfrastructure.Enums;
using Revit.DAL.Processing;
using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Families.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Revit.DML;
using Revit.Families.Rendering.Enums;
using Revit.Services.Allocation.Common;

namespace Revit.Services.Allocation
{
    public class ModelItemsAllocationService
    {
        private readonly IDataContext _dataContext;
        
        private readonly FamilyInstanceAllocationService _familyInstanceAllocationService;
        private readonly Sample _sample;

        public ModelItemsAllocationService(
            IFactory<Document, IDataContext> dataContextFactory,
            IFactory<Document, Sample> sampleFactory,
            FamilyInstanceAllocationService familyInstanceAllocationService,
            Document document)
        {
            _familyInstanceAllocationService = familyInstanceAllocationService;
            _sample = sampleFactory.New(document);
            _dataContext = dataContextFactory.New(document);
        }

        public int AllocateFoo()
        {
            var allocatedItems = Array.Empty<int>();

            try
            {
                var familySymbol = _sample?.Render(typeof(Foo)) 
                                   ?? throw new InvalidOperationException(@"Family symbol didn't get.");

                allocatedItems = _familyInstanceAllocationService
                    .PlaceInstances(familySymbol, int.MaxValue)
                    .ToArray();

                if (allocatedItems.Length > 0)
                {
                    var allocatedEntities = allocatedItems.Select(x =>
                        new Foo
                        {
                            Id = x, 
                            Name = $"Foo{x}", 
                            Description = @"What ever you want"
                        });

                    foreach (var allocatedEntity in allocatedEntities)
                    {
                        _dataContext.Foo.Attach(allocatedEntity);
                    }

                    _dataContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"{ex}");
            }

            return allocatedItems.Length;
        }

        public int AllocateBar()
        {
            var allocatedItems = Array.Empty<int>();

            try
            {
                var familySymbol = _sample?.Render(typeof(Bar))
                                   ?? throw new InvalidOperationException(@"Family symbol didn't get.");

                allocatedItems = _familyInstanceAllocationService
                    .PlaceInstances(familySymbol, int.MaxValue)
                    .ToArray();

                if (allocatedItems.Length > 0)
                {
                    var allocatedEntities = allocatedItems.Select(x =>
                        new Bar
                        {
                            Id = x,
                            Name = $"Bar{x}",
                            Description = @"What ever you want"
                        });

                    foreach (var allocatedEntity in allocatedEntities)
                    {
                        _dataContext.Bar.Attach(allocatedEntity);
                    }

                    _dataContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"{ex}");
            }

            return allocatedItems.Length;
        }
    }
}
