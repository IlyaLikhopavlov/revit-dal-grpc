using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class BarConverter : IRevitInstanceConverter<Bar, FamilyInstance>
    {
        private readonly ExtensibleStorage<BarSchema> _extensibleStorage;

        public BarConverter(
            ExtensibleStorage<BarSchema> extensibleStorage,
            Document document)
        {
            _extensibleStorage = extensibleStorage;
        }

        public void PushToRevit(FamilyInstance revitElement, Bar modelElement)
        {
            if (!string.IsNullOrEmpty(modelElement.Name) && revitElement.Name != modelElement.Name)
            {
                revitElement.Name = modelElement.Name;
            }

            //How to send data to family instance parameters?
            //var param1 = revitElement.LookupParameter(ParametersNames.Param1);
            //param1.SetValueString("Parameter Value");

            var schema = new BarSchema
            {
                Data = JsonSerializer.Serialize(modelElement)
            };

            _extensibleStorage.UpdateEntity(revitElement, schema);
        }

        public Bar PullFromRevit(FamilyInstance revitElement)
        {
            var barEntity = _extensibleStorage.GetEntity(revitElement);

            if (barEntity.Data == null)
            {
                return null;
            }

            var bar = JsonSerializer.Deserialize<Bar>(barEntity.Data);

            if (bar is null)
            {
                return null;
            }

            bar.Id = revitElement.Id.IntegerValue;

            return bar;
        }
    }
}
