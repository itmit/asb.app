using Android.OS;
using itmit.asb.app.Interface;

namespace itmit.asb.app.Droid
{
	public class CloseAplication : IClose
	{
		#region IClose members
		public void CloseApp()
		{
			Process.KillProcess(Process.MyPid());
		}
		#endregion
	}
}
