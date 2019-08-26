using System;
using Android.OS;
using itmit.asb.app.Droid;
using itmit.asb.app.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationTrackingServiceDroid))]
namespace itmit.asb.app.Droid
{
	public class LocationTrackingServiceDroid : ILocationTrackingService
	{
		public LocationTrackingServiceDroid()
		{ }

		public void StartService(Guid bidGuid)
		{
			var bundle = new Bundle();
			bundle.PutString("BidGuid", bidGuid.ToString());
			MainActivity.StartForegroundServiceCompat<LocationTrackingService>(Android.App.Application.Context, bundle);
		}
	}
}
