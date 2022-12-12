using App.Main;
using App.Main.UiFixtures;
using Microsoft.Extensions.DependencyInjection;

var startUp = new StartUp();
startUp.Build();

await StartUp.ServiceProvider.GetService<App.Services.Grpc.RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification()!;

await StartUp.ServiceProvider.GetService<CommandProcessor>()?.Run()!;

startUp.ShutDown();
