using Autodesk.Revit.UI;

namespace Revit.Services.ExternalEvents.Infrastructure
{
    public interface IExternalServiceEventHandler : IExternalEventHandler
    {
        ExternalEvent ExternalEvent { get; set; }

        void SetRequest(RequestObject request);
    }
}
