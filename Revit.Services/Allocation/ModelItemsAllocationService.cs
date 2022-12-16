using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Families.Rendering;
using Revit.Services.Allocation.Common;
using Revit.Services.Grpc.Services;

namespace Revit.Services.Allocation
{
    public class ModelItemsAllocationService
    {

        private readonly FamilyInstanceAllocationService _familyInstanceAllocationService;
        private readonly SampleRendering _sampleRendering;

        public ModelItemsAllocationService(
            IFactory<Document, SampleRendering> sampleFactory,
            FamilyInstanceAllocationService familyInstanceAllocationService,
            Document document)
        {
            _familyInstanceAllocationService = familyInstanceAllocationService;
            _sampleRendering = sampleFactory.New(document);
        }

        public int[] AllocateInstance(DomainModelTypesEnum instanceType)
        {
            var allocatedItems = Array.Empty<int>();

            try
            {
                var familySymbol = _sampleRendering?.Render(instanceType)
                                   ?? throw new InvalidOperationException(@"Family symbol didn't get.");

                allocatedItems = _familyInstanceAllocationService
                    .PlaceInstances(familySymbol, int.MaxValue)
                    .ToArray();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Allocation process failed", $"{ex}");
            }

            return allocatedItems;
        }

        public int[] AllocateFoo()
        {
            return AllocateInstance(DomainModelTypesEnum.Foo);
        }

        public int[] AllocateBar()
        {
            return AllocateInstance(DomainModelTypesEnum.Bar);
        }
    }
}
