using App.Main;
using Microsoft.Extensions.DependencyInjection;

var startUp = new StartUp();
startUp.Build();

await StartUp.ServiceProvider.GetService<App.Services.Grpc.RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification()!;

Console.ReadLine();

startUp.ShutDown();
