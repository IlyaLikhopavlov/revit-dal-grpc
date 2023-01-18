using App.DAL.DataContext;
using App.DML;
using App.Main;
using App.ScopedServicesFunctionality;
using App.Services.Grpc;
using App.Services.Revit;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Microsoft.Extensions.DependencyInjection;

var startUp = new StartUp();
startUp.Build();

StartUp.ServiceProvider.GetService<RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification();

Console.ReadLine();

var scopeFactory = StartUp.ServiceProvider.GetService<IDocumentDescriptorServiceScopeFactory>();
var documentDescriptor = StartUp.ServiceProvider.GetService<RevitApplication>()?.ActiveDocument;

if (documentDescriptor is not null)
{
    var scope = scopeFactory?.CreateScope(documentDescriptor);

    var grpcClient = scope?.ServiceProvider.GetService<RevitExtraDataExchangeClient>();
    var allocated = await grpcClient?.Allocate(typeof(Foo))!;

    var dataContext = scope?.ServiceProvider.GetService<IFactory<DocumentDescriptor, IDataContext>>()?.New(documentDescriptor);

    foreach (var id in allocated)
    {
        dataContext?.Foo.Attach(new Foo { Id = id, Name = $@"Foo {id}", Description = @"description" });
    }

    await dataContext?.SaveChanges()!;

    var foos = dataContext.Foo.Entries.Select(x => x.Entity);

    foreach (var foo in foos)
    {
        Console.WriteLine(foo.Id);
    }
}

Console.ReadLine();

startUp.ShutDown();
