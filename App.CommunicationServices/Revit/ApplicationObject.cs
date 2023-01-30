using App.CommunicationServices.Revit.Enums;
using App.CommunicationServices.Revit.EventArgs;

namespace App.CommunicationServices.Revit
{
    public class ApplicationObject
    {
        private DocumentDescriptor _documentDescriptor;

        public EventHandler<DocumentDescriptorChangedEventArgs> DocumentDescriptorChanged { get; set; }

        public DocumentDescriptor ActiveDocument
        {
            get => _documentDescriptor;
            set
            {
                _documentDescriptor = value;
                DataStatus = DataStatusEnum.Reliable;
            }
        }

        public DataStatusEnum DataStatus { get; private set; }

        public void SetDataStatusUntrusted()
        {
            DataStatus = DataStatusEnum.Untrusted;
        }
    }
}
