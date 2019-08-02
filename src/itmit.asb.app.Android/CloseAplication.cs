using itmit.asb.app.Interface;

namespace itmit.asb.app.Droid
{
    public class CloseAplication : IClose
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}