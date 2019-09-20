
using System;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Work;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = itmit.asb.app.Models.Location;

[assembly: Dependency(typeof(LocationUpdatesService))]

namespace itmit.asb.app.Droid.Services
{
	public class LocationUpdatesService : Worker
	{
		#region Data
		#region Fields
		private readonly string _tag = typeof(LocationTrackingService).FullName;
		private readonly LocationService _locationService = new LocationService();
		private Result _result = Result.InvokeSuccess();
		#endregion
		#endregion

		#region .ctor
		public LocationUpdatesService(Context context, WorkerParameters workerParameters)
			: base(context, workerParameters)
		{
		}
		#endregion

		#region Overrided
		public override Result DoWork()
		{
			if (App.User == null)
			{
				return Result.InvokeFailure();
			}

			Update();
			return Result.InvokeSuccess();
		}
		#endregion

		#region Private
		private async void Update()
		{
			Log.Debug(_tag, $"Begin update at {DateTime.Now};");

			if (Looper.MyLooper() == null)
			{
				Looper.Prepare();
			}

			var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default));

			if (location == null)
			{
				return;
			}

			Log.Debug(_tag, $"SUCCESS location received; latitude: {location.Latitude}; longitude:{location.Longitude}");

			var res = await _locationService.UpdateCurrentLocationTask(
						  new Location(location.Latitude, location.Longitude), App.User.UserToken);

			if (res)
			{
				_result = Result.InvokeSuccess();
				Log.Debug(_tag, $"Update location is SUCCESS at {DateTime.Now};");
			}
			else
			{
				_result = Result.InvokeFailure();
				Log.Debug(_tag, $"Update location is FAIL at {DateTime.Now}; Error: {_locationService.LastError}");
			}
		}
		#endregion
	}
}
