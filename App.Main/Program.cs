using App.Main;

var startUp = new StartUp();
startUp.Build();

var prompt = string.Empty;

while (prompt?.ToLower() != "exit")
{
    Console.Clear();
    Console.Write(@"Type ""Exit"" for program termination: ");
    prompt = Console.ReadLine();
}

startUp.ShutDown();
