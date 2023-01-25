using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Constants;

namespace Revit.Storage.InstancesAccess
{
    public static class RevitItemsProvider
    {
        public static Guid DetermineSchemaGuid(DomainModelTypesEnum entityType) => 
            entityType switch
            {
                DomainModelTypesEnum.Bar => new Guid(RevitStorage.BarSchemaGuid),
                DomainModelTypesEnum.Foo => new Guid(RevitStorage.FooSchemaGuid),
                _ => throw new InvalidEnumArgumentException(nameof(entityType))
            };

        public static List<FamilyInstance> GetInstancesByIdList(this Document document, IEnumerable<int> ids)
        {
            using var collector = new FilteredElementCollector(document, ids.Select(x => new ElementId(x)).ToArray());

            return collector
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .ToList();
        }

        public static List<FamilyInstance> GetInstancesByType(
            this Document document, 
            DomainModelTypesEnum entityType, 
            IEnumerable<int> elementIds = null)
        {
            using var collector = elementIds == null
                ? new FilteredElementCollector(document)
                : new FilteredElementCollector(document, elementIds.Select(x => new ElementId(x)).ToList());

            return collector
                .OfClass(typeof(FamilyInstance))
                .WherePasses(new ExtensibleStorageFilter(DetermineSchemaGuid(entityType)))
                .OfType<FamilyInstance>()
                .ToList();
        }
    }
}
