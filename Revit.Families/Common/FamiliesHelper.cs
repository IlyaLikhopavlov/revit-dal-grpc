using Autodesk.Revit.DB;

namespace Revit.Families.Common
{
    public static class FamiliesHelper
    {
        public static FamilySymbol[] GetFamilySymbols(this Document document, string familyName)
        {
            using var collector = new FilteredElementCollector(document);
            return collector
                .OfClass(typeof(FamilySymbol))
                .Select(n => (FamilySymbol)n)
                .Where(n => n.FamilyName == familyName)
                .ToArray();
        }

        public static FamilySymbol GetFamilySymbol(this Document document, string familyName, string symbolName)
        {
            using var collector = new FilteredElementCollector(document);
            return collector
                .OfClass(typeof(FamilySymbol))
                .Select(n => (FamilySymbol)n)
                .FirstOrDefault(n => n.FamilyName == familyName
                                        && n.Name == symbolName);
        }

        public static bool IsFamilyExists(this Document document, string familyName)
        {
            using var collector = new FilteredElementCollector(document);
            return collector
                .OfClass(typeof(FamilySymbol))
                .Select(n => (FamilySymbol)n)
                .Any(n => n.FamilyName == familyName);
        }

        public static FamilyInstance[] GetFamilyInstances(this Document document, string familyName, ElementId elementId)
        {
            using var collector = new FilteredElementCollector(document, elementId);
            return collector
                .OfClass(typeof(FamilyInstance))
                .Select(n => (FamilyInstance)n)
                .Where(n => n.Symbol.FamilyName == familyName)
                .ToArray();
        }

        public static List<Family> GetFamilies(this Document document)
        {
            using var collector = new FilteredElementCollector(document);
            return collector
                .OfClass(typeof(Family))
                .Cast<Family>()
                .ToList();
        }

        public static List<ReferencePlane> GetReferencePlanes(this Document document)
        {
            using var collector = new FilteredElementCollector(document);
            return collector
                .WhereElementIsNotElementType()
                .OfClass(typeof(ReferencePlane))
                .Select(n => (ReferencePlane)n)
                .ToList();
        }
    }
}
