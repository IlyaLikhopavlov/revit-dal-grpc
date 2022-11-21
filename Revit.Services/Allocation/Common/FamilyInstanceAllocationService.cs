using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Revit.DAL.Processing;
using Revit.DAL.Utils;

namespace Revit.Services.Allocation.Common
{
    public class FamilyInstanceAllocationService
    {
        private int _instancesForPlacementNumber;
        private List<int> _placedInstances;

        private ApplicationProcessing _applicationProcessing;
        private readonly IServiceProvider _serviceProvider;

        public FamilyInstanceAllocationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<int> PlaceInstances(FamilySymbol familySymbol, int instancesNumber)
        {
            _applicationProcessing ??= _serviceProvider.GetService<ApplicationProcessing>();
            
            if (instancesNumber <= 0)
            {
                return new List<int>();
            }

            _instancesForPlacementNumber = instancesNumber;
            _placedInstances = new List<int>();

            _applicationProcessing.UiApplication.Application.DocumentChanged += OnDocumentChanged;

            try
            {
                _applicationProcessing.UiApplication.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return _placedInstances;
            }
            finally
            {
                _applicationProcessing.UiApplication.Application.DocumentChanged -= OnDocumentChanged;
            }

            return _placedInstances;
        }

        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            var addedElementsIds = e.GetAddedElementIds();

            if (!addedElementsIds.Any())
            {
                return;
            }

            if (addedElementsIds.Count > 1)
            {
            }

            var addedItemId = addedElementsIds.FirstOrDefault()?.IntegerValue;

            if (addedItemId is not null and not 0)
            {
                _placedInstances.Add(addedItemId.Value);
            }

            var uiApplication = _applicationProcessing.UiApplication;

            if (_placedInstances.Count < _instancesForPlacementNumber)
            {
                Press.PressKey(uiApplication, Press.Keys.Esc);
                return;
            }

            Press.PressKey(uiApplication, Press.Keys.Esc);
            Press.PressKey(uiApplication, Press.Keys.Esc);
        }
    }
}
