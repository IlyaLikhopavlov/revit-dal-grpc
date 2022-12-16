using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

namespace App.Grpc.Bundle.ScopedServicesFunctionality
{
    internal class DocumentDescriptorScope : Scope<DocumentDescriptor>
    {
        public DocumentDescriptorScope(IServiceScope serviceScope, DocumentDescriptor t) : base(serviceScope, t)
        {
        }
    }
}
