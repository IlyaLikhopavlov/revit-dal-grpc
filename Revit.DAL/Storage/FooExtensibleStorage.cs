using Revit.DAL.Constants;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;

namespace Revit.DAL.Storage
{
    // в принципе можно и фабрику замутить
    public class FooExtensibleStorage : ExtensibleStorage<FooSchema>
    {
        public FooExtensibleStorage() : base(RevitStorage.FooSchemaGuid)
        {
        }
    }
}
