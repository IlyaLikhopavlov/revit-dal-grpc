using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.Services.Grpc.Services;

namespace App.Services.Revit
{
    public class RevitApplication
    {
        private DocumentDescriptor _documentDescriptor;

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
