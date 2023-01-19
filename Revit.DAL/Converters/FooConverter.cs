using App.CommunicationServices.Grpc;
using App.DAL.Converters.Common;
using App.DML;

namespace App.DAL.Converters
{
    public class FooConverter : RevitInstanceConverter<Foo>
    {
        public FooConverter(RevitExtraDataExchangeClient client, DocumentDescriptor documentDescriptor) : base(client, documentDescriptor)
        {
        }

        protected override void PushParametersToRevit(Foo modelElement)
        {
        }

        protected override void PullParametersFromRevit(ref Foo modelElement)
        {
        }
    }
}
