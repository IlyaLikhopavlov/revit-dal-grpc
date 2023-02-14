using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
