using App.DAL.Converters.Common;
using App.Services.Grpc;
using Revit.DML;

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
