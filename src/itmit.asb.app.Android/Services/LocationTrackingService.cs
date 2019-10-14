using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Java.Lang;
using Java.Util;
using Xamarin.Essentials;
using Exception = System.Exception;
using Location = itmit.asb.app.Models.Location;
using NetworkAccess = Xamarin.Essentials.NetworkAccess;

namespace itmit.asb.app.Droid.Services
{
	/// <summary>
	/// This is a sample started service. When the service is started, it will log a string that details how long
	/// the service has been running (using Android.Util.Log). This service displays a notification in the notification
	/// tray while the service is active.
	/// </summary>
	[Service]
	public class LocationTrackingService : Service
	{
		#region Data
		#region Fields
		private bool _isStarted;
		private readonly string _tag = typeof(LocationTrackingService).FullName;
		private Handler _handler;
		private Action _runnable;
		private const long Milliseconds = 5000;
		private const double MinimumDistance = 0;
		private bool _wasReset;
		private DateTime _startTime;
		private readonly ILocationService _locationService = new LocationService();
		private Guid _bidGuid;
		private UserToken _token;
		private bool _isGuard;
		private LocationManager _locationManager;
		private string _locationProvider;
		private LocationListener _locationListener;
		#endregion
		#endregion

		#region Overrided
		public override IBinder OnBind(Intent intent) => null;

		public override void OnCreate()
		{
			base.OnCreate();
			Log.Info(_tag, "OnCreate: the service is initializing.");

			_handler = new Handler();
			_startTime = DateTime.UtcNow;

			_token = new UserToken
			{
				Token = (string)App.User.UserToken.Token.Clone()
			};
			_isGuard = App.User.IsGuard;

			Criteria criteria = new Criteria
			{
				Accuracy = Accuracy.Fine,
				AltitudeRequired = false,
				BearingRequired = false
			};

			_locationManager = Application.Context.GetSystemService(Context.LocationService) as LocationManager;

			if (_locationManager != null)
			{
				_locationProvider = _locationManager.GetBestProvider(criteria, true);
			}

			_locationListener = new LocationListener();

			if (_locationProvider != null)
			{
				Log.Verbose(_tag, "Location provider: " + _locationProvider);
			}
			else
			{
				Log.Error(_tag, "Location provider is null. Location events will not work.");
			}

			// This Action is only for demonstration purposes.
			_runnable = () =>
			{
				var duration = DateTime.UtcNow.Subtract(_startTime);
				var msg = _wasReset ? $"Service restarted at {_startTime} ({duration:c} ago)."
							  : $"Service started at {_startTime} ({duration:c} ago).";

				try
				{
					Update();
				}
				catch(WebException e)
				{
					Log.Error(_tag, e.Message);
				}
				catch (Exception e)
				{
					Log.Error(_tag, e.Message);
				}

				Log.Debug(_tag, msg);
				var i = new Intent(Constants.NotificationBroadcastAction);
				i.PutExtra(Constants.BroadcastMessageKey, msg);

				LocalBroadcastManager.GetInstance(this)
									 .SendBroadcast(i);

				_handler.PostDelayed(_runnable, Constants.DelayBetweenLogMessages);
			};
		}

		public override void OnDestroy()
		{
			var duration = DateTime.UtcNow.Subtract(_startTime);
			var msg = _wasReset ? $"Service restarted at {_startTime} ({duration:c} ago)."
						  : $"Service started at {_startTime} ({duration:c} ago).";

			// We need to shut things down.
			Log.Info(_tag, "OnDestroy: The started service is shutting down.");

			// Stop the handler.
			_handler.RemoveCallbacks(_runnable);

			// Remove the notification from the status bar.
			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Cancel(Constants.ServiceRunningNotificationId);

			_wasReset = false;
			_isStarted = false;
			base.OnDestroy();
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action.Equals(Constants.ActionStartService))
			{
				if (_isStarted)
				{
					Log.Info(_tag, "OnStartCommand: The service is already running.");
				}
				else
				{
					Guid.TryParse(intent.GetStringExtra("BidGuid"), out _bidGuid);

					Log.Info(_tag, "OnStartCommand: The service is starting.");
					RegisterForegroundService();
					_handler.PostDelayed(_runnable, Constants.DelayBetweenLogMessages);
					_isStarted = true;
				}
			}
			else if (intent.Action.Equals(Constants.ActionStopService))
			{
				Log.Info(_tag, "OnStartCommand: The service is stopping.");
				_wasReset = false;
				StopForeground(true);
				StopSelf();
				_isStarted = false;
			}
			else if (intent.Action.Equals(Constants.ActionRestartTimer))
			{
				Log.Info(_tag, "OnStartCommand: Restarting the timer.");

				_wasReset = true;
			}

			// This tells Android not to restart the service if it is killed to reclaim resources.
			return StartCommandResult.Sticky;
		}
		#endregion

		#region Private
		private async void Update()
		{
			if (_bidGuid.Equals(Guid.Empty))
			{
				Log.Error(_tag, "Bid Guid is empty.");
			}

			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
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

				if (location == null || _token == null)
				{
					return;
				}

				bool res;
				if (_isGuard)
				{

					res = await UpdateCurrentLocation(new Location(location.Latitude, location.Longitude), _token, _bidGuid);

				}
				else
				{
					res = await AddPointOnMapTask(
							  new Location(location.Latitude, location.Longitude), _token, _bidGuid);
				}

				Log.Debug(_tag, res ? $"Update location is SUCCESS at {DateTime.Now};" : $"Update location is FAIL at {DateTime.Now}; Error: {_locationService.LastError}");
			}
		}

		/// <summary>
		/// Адрес для API обновления местоположения пользователя.
		/// </summary>
		private const string UpdateCurrentLocationUri = "http://lk.asb-security.ru/api/client/updateCurrentLocation";
		private async Task<bool> UpdateCurrentLocation(Location location, UserToken token, Guid bidGuid)
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

		private async Task<bool> AddPointOnMapTask(Location location, UserToken token, Guid bidGuid)
		{
			using (var client = new HttpClient(new CustomAndroidClientHandler()))
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.Token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new Dictionary<string, string>
				{
					{
						"latitude", location.Latitude.ToString(CultureInfo.InvariantCulture)
					},
					{
						"longitude", location.Longitude.ToString(CultureInfo.InvariantCulture)
					}
				};

				if (bidGuid != Guid.Empty)
				{
					encodedContent.Add("uid", bidGuid.ToString());
				}

				var response = await client.PostAsync("http://lk.asb-security.ru/api/pointOnMap", new FormUrlEncodedContent(encodedContent));
				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Builds a PendingIntent that will display the main activity of the app. This is used when the
		/// user taps on the notification; it will take them to the main activity of the app.
		/// </summary>
		/// <returns>The content intent.</returns>
		private PendingIntent BuildIntentToShowMainActivity()
		{
			var notificationIntent = new Intent(this, typeof(MainActivity));
			notificationIntent.SetAction(Constants.ActionMainActivity);
			notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
			notificationIntent.PutExtra(Constants.ServiceStartedKey, true);

			var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
			return pendingIntent;
		}

		/// <summary>
		/// Builds a Notification.Action that will instruct the service to restart the timer.
		/// </summary>
		/// <returns>The restart timer action.</returns>
		private Notification.Action BuildRestartTimerAction()
		{
			var restartTimerIntent = new Intent(this, GetType());
			restartTimerIntent.SetAction(Constants.ActionRestartTimer);
			var restartTimerPendingIntent = PendingIntent.GetService(this, 0, restartTimerIntent, 0);

			var builder = new Notification.Action.Builder(Android.Resource.Drawable.EditText, "Restart Timer", restartTimerPendingIntent);

			return builder.Build();
		}

		/// <summary>
		/// Builds the Notification.Action that will allow the user to stop the service via the
		/// notification in the status bar
		/// </summary>
		/// <returns>The stop service action.</returns>
		private Notification.Action BuildStopServiceAction()
		{
			var stopServiceIntent = new Intent(this, GetType());
			stopServiceIntent.SetAction(Constants.ActionStopService);
			var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

			var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause, "Stop Service", stopServicePendingIntent);
			return builder.Build();
		}

		private void CreateNotificationChannel()
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.O)
			{
				// Notification channels are new in API 26 (and not a part of the
				// support library). There is no need to create a notification
				// channel on older versions of Android.
				return;
			}

			var name = AppInfo.Name;
			var description = "description";
			var channel = new NotificationChannel(AppInfo.Name, name, NotificationImportance.Default)
			{
				Description = description
			};

			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.CreateNotificationChannel(channel);
		}

		private void RegisterForegroundService()
		{
			CreateNotificationChannel();
			var notification = new Notification.Builder(this, AppInfo.Name).SetContentTitle(AppInfo.Name)
															 .SetContentText("Отслеживание включено")
															 .SetSmallIcon(Android.Resource.Drawable.ButtonStar)
															 .SetContentIntent(BuildIntentToShowMainActivity())
															 .SetOngoing(true)
															 .AddAction(BuildStopServiceAction())
															 .Build();

			// Enlist this instance of the service as a foreground service
			StartForeground(Constants.ServiceRunningNotificationId, notification);
		}
		#endregion
	}
}
