using App.DAL.Converters.Common;
using App.DML;
using App.Services.Grpc;

namespace App.DAL.Converters
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
