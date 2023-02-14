using Autodesk.Revit.DB;

namespace Bimdance.Revit.Framework.RevitDocument
{
    public class DocumentEqualityComparer : IEqualityComparer<Document>
    {
        public bool Equals(Document x, Document y)
        {
            return x?.ProjectInformation.UniqueId == y?.ProjectInformation.UniqueId;
        }

        public int GetHashCode(Document obj)
        {
            return obj.GetHashCode();
        }
    }
}
