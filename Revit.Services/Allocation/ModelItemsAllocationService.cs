
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.DataContext;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Families.Rendering;
using Revit.DML;
using Revit.Services.Allocation.Common;
using Element = Revit.DML.Element;

namespace Revit.Services.Allocation
{
    public class ModelItemsAllocationService
    {
        private readonly IDataContext _dataContext;
        
        private readonly FamilyInstanceAllocationService _familyInstanceAllocationService;
        private readonly SampleRendering _sampleRendering;

        public ModelItemsAllocationService(
            IFactory<Document, IDataContext> dataContextFactory,
            IFactory<Document, SampleRendering> sampleFactory,
            FamilyInstanceAllocationService familyInstanceAllocationService,
            Document document)
        {
            _familyInstanceAllocationService = familyInstanceAllocationService;
            _sampleRendering = sampleFactory.New(document);
            _dataContext = dataContextFactory.New(document);
        }

        private int AllocateInstance<T>() where T : Element, new()
        {
            var allocatedItems = Array.Empty<int>();

            try
            {
                var familySymbol = _sampleRendering?.Render(typeof(T))
                                   ?? throw new InvalidOperationException(@"Family symbol didn't get.");

                allocatedItems = _familyInstanceAllocationService
                    .PlaceInstances(familySymbol, int.MaxValue)
                    .ToArray();

                if (allocatedItems.Length > 0)
                {
                    var allocatedEntities = allocatedItems.Select(x =>
                        new T
                        {
                            Id = x,
                            Name = $"{typeof(T).Name}{x}",
                        });

                    foreach (var allocatedEntity in allocatedEntities)
                    {
                        var switcher = new Dictionary<Type, Action<Element>>
                        {
                            { typeof(Foo), x => _dataContext.Foo.Attach((Foo)x) },
                            { typeof(Bar), x => _dataContext.Bar.Attach((Bar)x) }
                        };

                        if (switcher.TryGetValue(allocatedEntity.GetType(), out var attachAction))
                        {
                            attachAction.Invoke(allocatedEntity);
                        }
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

        public int AllocateFoo()
        {
            return AllocateInstance<Foo>();
        }

        public int AllocateBar()
        {
            return AllocateInstance<Bar>();
        }
    }
}
