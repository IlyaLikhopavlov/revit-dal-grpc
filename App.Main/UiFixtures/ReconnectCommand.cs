using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Grpc;

namespace App.Main.UiFixtures
{
    internal class ReconnectCommand : ICommand
    {
        private readonly RevitActiveDocumentNotificationClient _client;

        public ReconnectCommand(RevitActiveDocumentNotificationClient client)
        {
            _client = client;
        }

        public async Task Execute()
        {
            await _client.RunGettingRevitNotification();
        }
    }
}
