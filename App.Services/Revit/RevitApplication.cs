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
        public DocumentDescriptor ActiveDocument { get; set; }

        public CurrentDocumentStatusEnum Status { get; set; }
    }
}
