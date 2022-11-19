using Revit.DAL.Constants;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.DAL.Storage
{
    public class BarExtensibleStorage : ExtensibleStorage<BarSchema>
    {
        public BarExtensibleStorage() : base(RevitStorage.BarSchemaGuid)
        {
        }
    }
}
