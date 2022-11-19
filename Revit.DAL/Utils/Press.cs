using System.Runtime.InteropServices;
using Autodesk.Revit.UI;

namespace Revit.DAL.Utils
{
    public static class Press
    {
        [DllImport("USER32.DLL")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        public static void PressKey(UIApplication uiApplication, Keys key)
        {
            PostMessage(uiApplication.MainWindowHandle, (uint)KeyboardMsg.WmKeydown, (uint)Keys.Esc, 0);
        }

        public enum KeyboardMsg : uint
        {
            WmKeydown = 0x100,
            WmKeyup = 0x101
        }

        public enum Keys
        {
            Esc = 0x1B
        }
    }
}
