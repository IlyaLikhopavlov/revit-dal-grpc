using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Constants;

namespace Revit.DAL.DataContext.RevitItems
{
    public static class RevitItemsProvider
    {
        //public static List<FamilyInstance> GetAutomationItemFamilyInstances(this Document document, IEnumerable<ElementId> elementIds = null)
        //{
        //    var result = new List<FamilyInstance>();

        //    var ids = elementIds?.ToList();
        //    result.AddRange(document.GetFooInstances(ids));
        //    result.AddRange(document.GetBarInstances(ids));

        //    return result;
        //}

        //public static List<FamilyInstance> GetBarInstances(this Document document, IEnumerable<ElementId> elementIds = null)
        //{
        //    using var collector = elementIds == null
        //        ? new FilteredElementCollector(document)
        //        : new FilteredElementCollector(document, elementIds.ToList());

        //    return collector
        //        .OfClass(typeof(FamilyInstance))
        //        .WherePasses(new ExtensibleStorageFilter(new Guid(RevitStorage.BarSchemaGuid)))
        //        .OfType<FamilyInstance>()
        //        .ToList();
        //}

        public static List<FamilyInstance> GetFooInstances(this Document document, IEnumerable<ElementId> elementIds = null)
        {
            using var collector = elementIds == null
                       ? new FilteredElementCollector(document)
                       : new FilteredElementCollector(document, elementIds.ToList());

            return collector
                .OfClass(typeof(FamilyInstance))
                .WherePasses(new ExtensibleStorageFilter(new Guid(RevitStorage.FooSchemaGuid)))
                .OfType<FamilyInstance>()
                .ToList();
        }

        public static ProjectInfo GetProjectInfo(this Document document)
        {
            return document?.ProjectInformation;
        }

        public static List<FamilySymbol> GetFamilySymbols(this Document document)
        {
            using var collector = new FilteredElementCollector(document);

            return collector
                .OfClass(typeof(FamilySymbol))
                .Select(n => (FamilySymbol)n)
                .ToList();
        }

        
    }
}
