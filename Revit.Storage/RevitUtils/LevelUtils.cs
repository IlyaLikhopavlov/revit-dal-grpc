using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.Storage.RevitUtils;

public static class LevelUtils
{
    public static IList<Level> GetLevels(this Document document)
    {
        using var collector = new FilteredElementCollector(document);
        return collector.OfCategory(BuiltInCategory.OST_Levels)
            .WhereElementIsNotElementType()
            .OfType<Level>().ToList();
    }

    public static IList<Room> GetRooms(this Document document, bool isPlacedOnly = false)
    {
        using var collector = new FilteredElementCollector(document);
        var allRooms = collector
            .OfCategory(BuiltInCategory.OST_Rooms)
            .OfType<Room>()
            .ToList();
        return isPlacedOnly
            ? allRooms.Where(n => n.Location != null).ToList()
            : allRooms;
    }

    public static IList<Room> GetLevelRooms(this Document document, Level level)
    {
        return document.GetRooms(isPlacedOnly: true)
            .Where(r => r.Level?.Id.IntegerValue == level.Id.IntegerValue)
            .Select(r => document.GetElementAs<Room>(r))
            .Where(r => r is { IsValidObject: true })
            //.Select(x => new RoomViewModel(x))
            .OrderBy(x => x.Number, new NumbersStringComparer())
            .ToList();
    }

    public static IList<ViewPlan> GetViewPlans(this Document document, Level level)
    {
        return level.GetDependentElements(new ElementClassFilter(typeof(ViewPlan)))
            .Select(x => document.GetElementAs<ViewPlan>(document.GetElement(x)))
            .Where(x => x is { IsValidObject: true })
            // .Select(x => new PlanViewModel(this, x))
            .OrderBy(x => x.Name, new NumbersStringComparer())
            .ToList();
    }

    public static T GetElementAs<T>(this Document document, Element element) => document.GetElement(element.Id) is T t ? t : default;

    public static IEnumerable<TElement> GetElementsInDocument<TElement>(this Document document)
        where TElement : Element
    {
        var collector = new FilteredElementCollector(document);
        var elements = collector.OfClass(typeof(TElement));

        foreach (var element in elements)
        {
            if (element is TElement tElement)
            {
                yield return tElement;
            }
        }
    }
}