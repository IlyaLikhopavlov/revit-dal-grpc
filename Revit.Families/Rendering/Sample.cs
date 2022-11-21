using Autodesk.Revit.DB;
using Revit.DAL.Utils;
using Revit.DML;
using Revit.Families.Common;

namespace Revit.Families.Rendering
{
    public class Sample
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

        public Sample(Document document)
        {
            _document = document;
        }

        public FamilySymbol Render(Type type)
        {
            (string Family, string Resource) name;
            if (type == typeof(Foo))
            {
                name = (FooNames.Family, FooNames.Resource);
            }
            else if (type == typeof(Bar))
            {
                name = (BarNames.Family, BarNames.Resource);
            }
            else
            {
                throw new InvalidOperationException($"Family doesn't exist for the type {type.Name}");
            }

            var sampleSymbol = _document.GetFamilySymbols(name.Family).FirstOrDefault();
            if (sampleSymbol != null)
            {
                return sampleSymbol;
            }

            var familyData =  Families.ResourceManager.GetObject(name.Resource) as byte[];
            var tempFileName = $"{Path.GetTempFileName()}{FamilyExtension}";
            File.WriteAllBytes(tempFileName, familyData!);

            _document.ExecuteTransaction(() =>
            {
                _document.LoadFamily(tempFileName, new FamilyLoadOptions(), out var family);
                family.Name = name.Family;
            }, "Rename family");
            File.Delete(tempFileName);

            return _document.GetFamilySymbols(name.Family).FirstOrDefault();
        }
    }
}
