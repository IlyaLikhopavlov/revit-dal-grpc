using Autodesk.Revit.DB;
using Revit.DAL.Constants;
using Revit.DAL.DataContext.RevitItems;
using Revit.DAL.Storage.Infrastructure;

namespace Revit.DAL.Storage
{
    public class SettingsExtensibleStorage : ExtensibleStorageDictionary
    {
        public SettingsExtensibleStorage(Document currentDocument)
            : base(
                RevitStorage.SettingsExtensibleStorageSchemaGuid, 
                "SettingsExtensibleStorage", 
                "SettingsDictionary", 
                currentDocument.GetProjectInfo())
        {
        }
    }
}
