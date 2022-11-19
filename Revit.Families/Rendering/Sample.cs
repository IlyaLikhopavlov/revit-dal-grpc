using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DAL.Utils;
using Revit.Families.Common;
using Revit.Families.Rendering.Enums;

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
        private readonly Application _application;

        public Sample(UIApplication uiApplication)
        {
            _application = uiApplication.Application;
            _document = uiApplication.ActiveUIDocument.Document;
        }

        public FamilySymbol Render(FamilyTypeEnum familyType)
        {
            var name = familyType switch
            {
                FamilyTypeEnum.Foo => (FooNames.Family, FooNames.Resource),
                FamilyTypeEnum.Bar => (BarNames.Family, BarNames.Resource),
                _ => throw new ArgumentOutOfRangeException(nameof(familyType), familyType, null)
            };

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


            return _document.GetFamilySymbols(name.Family).First();
        }
    }
}
