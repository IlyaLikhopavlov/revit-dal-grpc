using Autodesk.Revit.UI;
using Grpc.Core;
using Grpc.Core.Logging;
using Revit.Services.Grpc.Services;

namespace Revit.Services.Grpc
{
    public class GrpcServerBootstrapper
    {
        private Server _server;

        private readonly RevitActiveDocumentNotificationService _revitActiveDocumentNotificationService;

        public GrpcServerBootstrapper(RevitActiveDocumentNotificationService revitActiveDocumentNotificationService)
        {
            _revitActiveDocumentNotificationService = revitActiveDocumentNotificationService;
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
                        RevitActiveDocumentNotification.BindService(_revitActiveDocumentNotificationService)
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
