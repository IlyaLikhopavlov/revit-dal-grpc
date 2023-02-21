using Autodesk.Revit.DB;
using Bimdance.Revit.Framework.RevitDocument;
using Revit.Families.Common;
using Revit.Services.Grpc.Services;

namespace Revit.Families.Rendering
{
    public class SampleRendering
    {
        private const string FamilyExtension = @".rfa";
        
        private static class FooNames
        {
            public const string Family = @"Revit.DAL.Foo";
            public const string Resource = @"RevitDalFoo";
        }

        private static class BarNames
        {
            public const string Family = @"Revit.DAL.Bar";
            public const string Resource = @"RevitDalBar";
        }

        private readonly Document _document;

        public SampleRendering(Document document)
        {
            _document = document;
        }

        public FamilySymbol Render(DomainModelTypesEnum instanceType)
        {
            var name = instanceType switch
            {
                DomainModelTypesEnum.Foo => (FooNames.Family, FooNames.Resource),
                DomainModelTypesEnum.Bar => (BarNames.Family, BarNames.Resource),
                _ => throw new ArgumentOutOfRangeException($"Family doesn't exist for the type {instanceType}")
            };

            var sampleSymbol = _document.GetFamilySymbols(name.Family).FirstOrDefault();
            if (sampleSymbol != null)
            {
                return sampleSymbol;
            }

            var familyData =  Families.ResourceManager.GetObject(name.Resource) as byte[];
            var tempFileName = $"{Path.GetTempFileName()}{FamilyExtension}";
            File.WriteAllBytes(tempFileName, familyData!);

            _document.SaveChanges(() =>
            {
                _document.LoadFamily(tempFileName, new FamilyLoadOptions(), out var family);
                family.Name = name.Family;
            });
            File.Delete(tempFileName);

            return _document.GetFamilySymbols(name.Family).FirstOrDefault();
        }
    }
}
