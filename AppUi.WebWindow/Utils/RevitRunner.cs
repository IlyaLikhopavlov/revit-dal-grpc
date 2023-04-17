using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AppUi.WebWindow.Utils
{
    internal class RevitRunner
    {
        public void KillRevit()
        {
            foreach (var process in Process.GetProcessesByName("Revit"))
            {
                process.Kill(true);
            }
        }

        public void RunRevit()
        {
            var revitProcess = new Process();
            revitProcess.StartInfo.FileName = "C:\\Program Files\\Autodesk\\Revit 2019\\Revit.exe";
            revitProcess.StartInfo.Arguments = "\"C:\\Users\\Ilya Likhopavlov\\Documents\\FooBar_test.rvt\"";
            revitProcess.Start();

            while (!revitProcess.MainWindowTitle.Contains("FooBar_test.rvt"))
            {
                Thread.Sleep(1000);
                revitProcess.Refresh();
            }
            Thread.Sleep(10000);
        }

        public static void DisableBimCd()
        {
            try
            {
                System.IO.File.Move(
                    @"C:\ProgramData\Autodesk\Revit\Addins\2019\BimControlDesign.addin",
                    @"C:\ProgramData\Autodesk\Revit\Addins\2019\BimControlDesign.addin_");
            }
            catch
            {

            }
        }

        public static void EnableBimCd()
        {
            try
            {
                System.IO.File.Move(
                    @"C:\ProgramData\Autodesk\Revit\Addins\2019\BimControlDesign.addin_",
                    @"C:\ProgramData\Autodesk\Revit\Addins\2019\BimControlDesign.addin");
            }
            catch
            {

            }
        }
    }
}
