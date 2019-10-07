
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using AndroidX.Work;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Java.Lang;
using Xamarin.Essentials;
using Xamarin.Forms;
using ILocationListener = Android.Locations.ILocationListener;
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
		private LocationManager _locationManager;
		private string _locationProvider;
		private ILocationListener _locationListener;
		private const long Milliseconds = 5000;
		private const double MinimumDistance = 5.5;
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
			Criteria criteria = new Criteria
			{
				Accuracy = Accuracy.Fine,
				AltitudeRequired = false,
				BearingRequired = false
			};

			_locationManager = Android.App.Application.Context.GetSystemService(Context.LocationService) as LocationManager;

			if (App.User == null || _locationManager == null)
			{
				return Result.InvokeFailure();
			}
			
			_locationProvider = _locationManager.GetBestProvider(criteria, true);
			_locationListener = new LocationListener();

			if (_locationProvider != null)
			{
				Log.Verbose(_tag, "Location provider: " + _locationProvider);
			}
			else
			{
				Log.Error(_tag, "Location provider is null. Location events will not work.");

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

			try
			{
				_locationManager.RequestLocationUpdates(_locationProvider, Milliseconds, (float)MinimumDistance, _locationListener);
				//_locationManager.RequestLocationUpdates(_locationProvider, Milliseconds, a, _locationListener);
			}
			catch (SecurityException ex)
			{
				Log.Error(_tag, "fail to request location update, ignore", ex);
			}
			catch (IllegalArgumentException ex)
			{
				Log.Error(_tag, "network provider does not exist, " + ex.Message);
			}

			var location = _locationManager.GetLastKnownLocation(_locationProvider);

			if (location == null)
			{
				Log.Debug(_tag, "FAILED location received; location is null");
				return;
			}

			Log.Debug(_tag, $"SUCCESS location received; latitude: {location.Latitude}; longitude:{location.Longitude}");

			var res = await UpdateCurrentLocationTask(
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

		/// <summary>
		/// Адрес для API обновления местоположения пользователя.
		/// </summary>
		private const string UpdateCurrentLocationUri = "http://lk.asb-security.ru/api/client/updateCurrentLocation";

		private async Task<bool> UpdateCurrentLocationTask(Location location, UserToken token)
		{

			using (var client = new HttpClient(new CustomAndroidClientHandler()))
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.Token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(UpdateCurrentLocationUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "latitude", location.Latitude.ToString(CultureInfo.InvariantCulture)
														  },
														  {
															  "longitude", location.Longitude.ToString(CultureInfo.InvariantCulture)
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();

				System.Diagnostics.Debug.WriteLine(jsonString);

				var result = await Task.FromResult(response.IsSuccessStatusCode);

				return result;
			}
		}
		#endregion
	}
}
