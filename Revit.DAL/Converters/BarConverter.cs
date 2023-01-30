using App.CommunicationServices.Grpc;
using App.DAL.Revit.Converters.Common;
using App.DML;

namespace App.DAL.Revit.Converters
{
    public class BarConverter : RevitInstanceConverter<Bar>
    {
        public BarConverter(RevitExtraDataExchangeClient client, DocumentDescriptor documentDescriptor) : base(client, documentDescriptor)
        {
        }

        protected override void PushParametersToRevit(Bar modelElement)
        {
        }

        protected override void PullParametersFromRevit(ref Bar modelElement)
        {
        }
    }
}
