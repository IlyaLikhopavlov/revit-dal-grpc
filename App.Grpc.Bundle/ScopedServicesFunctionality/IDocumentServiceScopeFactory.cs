using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Revit.Services.Grpc.Services;

namespace App.Grpc.Bundle.ScopedServicesFunctionality
{
    public interface IDocumentServiceScopeFactory
    {
        IServiceScope CreateScope(DocumentDescriptor documentDescriptor);

        void OnDocumentClosing(DocumentDescriptor documentDescriptor);
    }
}
