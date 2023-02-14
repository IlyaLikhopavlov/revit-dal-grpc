namespace App.CommunicationServices.Utils.Comparers
{
    public class DocumentDescriptorEqualityComparer : IEqualityComparer<DocumentDescriptor>
    {
        public bool Equals(DocumentDescriptor x, DocumentDescriptor y)
        {
            return x?.Id == y?.Id && x?.Title == y?.Title;
        }

        public int GetHashCode(DocumentDescriptor obj)
        {
            return HashCode.Combine(obj.Id, obj.Title);
        }
    }
}
