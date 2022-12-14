using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

namespace App.Grpc.Bundle.ScopedServicesFunctionality
{
    internal class DocumentDescriptorScope : IServiceScope
    {
        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        private readonly IServiceScope _serviceScope;

        public DocumentDescriptorScope(IServiceScope serviceScope, DocumentDescriptor document)
        {
            _serviceScope = serviceScope;
            DocumentDescriptor = document;
        }

        public DocumentDescriptor DocumentDescriptor { get; }

        public void Dispose()
        {
            _serviceScope?.Dispose();
        }
    }
}
