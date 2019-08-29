using System;
using AndroidX.Work;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Services;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationServiceDroid))]
namespace itmit.asb.app.Droid.Services
{
	public class LocationServiceDroid : ILocationUpdatesService
	{
		private UUID _workId;

		public LocationServiceDroid()
		{ }

		public void StopService()
		{
			WorkManager.Instance.CancelWorkById(_workId);
		}

		public void StartService()
		{
			PeriodicWorkRequest taxWorkRequest = PeriodicWorkRequest
												 .Builder
												 .From<LocationUpdatesService>(TimeSpan.FromMinutes(5)
												 ).Build();
			WorkManager.Instance.Enqueue(taxWorkRequest);
			_workId = taxWorkRequest.Id;
		}
	}
}
