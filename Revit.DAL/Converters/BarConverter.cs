using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class BarConverter : RevitInstanceConverter<Bar, FamilyInstance>
    {
        public BarConverter(
            IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document) : base(extensibleStorageFactory, document)
        {
        }

        protected override void SendParametersToRevit(FamilyInstance revitElement, Bar modelElement)
        {
        }

        protected override void ReceiveParametersFromRevit(FamilyInstance revitElement, ref Bar modelElement)
        {
        }
    }
}
