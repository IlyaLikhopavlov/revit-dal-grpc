using Autodesk.Revit.UI;
using Grpc.Core;
using Revit.Services.Grpc.Services;

namespace Revit.Services.Grpc
{
    public class GrpcServerBootstrapper
    {
        private Server _server;

        private readonly RevitActiveDocumentNotificationService _revitActiveDocumentNotificationService;
        private readonly RevitDataExchangeService _dataExchangeService;

        public GrpcServerBootstrapper(
            RevitActiveDocumentNotificationService revitActiveDocumentNotificationService,
            RevitDataExchangeService revitDataExchangeService)
        {
            _revitActiveDocumentNotificationService = revitActiveDocumentNotificationService;
            _dataExchangeService = revitDataExchangeService;
        }

        public bool StartServer(string address, int port)
        {
            if (_server != null)
            {
                return true;
            }

            try
            {
                _server = new Server
                {
                    Services =
                    {
                        RevitActiveDocumentNotification.BindService(_revitActiveDocumentNotificationService),
                        RevitDataExchange.BindService(_dataExchangeService)
                    },
                    Ports = { new ServerPort(address, port, ServerCredentials.Insecure) },
                };

                _server.Start();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Failure", $"GRPC server isn't started. {ex.Message}");
                return false;
            }

            return true;
        }

        public void StopServer()
        {
            _server?.ShutdownAsync().Wait();
        }
    }
}
