﻿using App.DAL.Common.Repositories.RevitRepositories.Generic;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.DataInfrastructure;
using App.DAL.Revit.DataContext.DataInfrastructure.Enums;
using App.DML;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.Initialization;

namespace App.DAL.Common.Repositories.RevitRepositories
{
    public class FooRevitRepository : GenericRevitRepository<Foo>, IFooRepository
    {
        public FooRevitRepository(
            IFactory<DocumentDescriptor, IDataContext> dataContextFactory, 
            DocumentDescriptor documentDescriptor) 
            : base(dataContextFactory, documentDescriptor)
        {
        }
    }
}
