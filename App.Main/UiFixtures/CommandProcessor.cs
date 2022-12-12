using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Main.UiFixtures
{
    public class CommandProcessor
    {
        private static readonly Dictionary<string, ICommand> Commands = new()
        {
            { "clear", StartUp.ServiceProvider.GetService<ClearCommand>() },
            { "reconnect", StartUp.ServiceProvider.GetService<ReconnectCommand>() }

        };

        private static string PrepareCommandPrompt(string commandPrompt) => commandPrompt.ToLower().Trim();

        public async Task Run()
        {
            var prompt = string.Empty;

            while (PrepareCommandPrompt(prompt) != "exit")
            {
                Console.Clear();
                Console.Write(@"Type ""Exit"" for program termination: ");
                prompt = Console.ReadLine();
                
                try
                {
                    await RecognizeAndExecute(prompt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static async Task RecognizeAndExecute(string commandPrompt)
        {
            if (Commands.TryGetValue(PrepareCommandPrompt(commandPrompt), out var command))
            {
                await command.Execute();
                return;
            }

            Console.WriteLine(@$"Command ""{commandPrompt}"" didn't find.");
        }
    }
}
