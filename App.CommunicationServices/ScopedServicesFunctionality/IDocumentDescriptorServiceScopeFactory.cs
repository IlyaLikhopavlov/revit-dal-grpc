﻿using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public interface IDocumentDescriptorServiceScopeFactory
    {
        IServiceScope CreateScope();

        T GetScopedService<T>() where T : class;

        object GetScopedService(Type serviceType, object constructorArg = null);

        void RemoveScope(DocumentDescriptor documentDescriptor);
    }
}
