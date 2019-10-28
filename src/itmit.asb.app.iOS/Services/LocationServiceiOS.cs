using System;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using itmit.asb.app.iOS.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = Xamarin.Essentials.Location;

[assembly: Dependency(typeof(LocationServiceIos))]
namespace itmit.asb.app.iOS.Services
{
	public class LocationServiceIos : ILocationUpdatesService
	{
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";

		/// <summary>
		/// Запускает сервис.
		/// </summary>
		public async void StartService()
		{
			nint taskId = 0;
			var taskEnded = false;
			var id = taskId;
			taskId = UIApplication.SharedApplication.BeginBackgroundTask(LocationUpdateService, () =>
			{
				//when time is up and task has not finished, call this method to finish the task to prevent the app from being terminated
				Console.WriteLine($"Background task '{LocationUpdateService}' got killed");
				taskEnded = true;
				UIApplication.SharedApplication.EndBackgroundTask(id);
			});
			await Task.Factory.StartNew(() =>
			{
				//here we run the actual task
				Console.WriteLine($"Background task '{LocationUpdateService}' started");
				MainThread.BeginInvokeOnMainThread(Action);
				taskEnded = true;
				UIApplication.SharedApplication.EndBackgroundTask(taskId);
				Console.WriteLine($"Background task '{LocationUpdateService}' finished");
			});

			await Task.Factory.StartNew(async () =>
			{
				//Just a method that logs how much time we have remaining. Usually a background task has around 180 seconds to complete. 
				while (!taskEnded)
				{
					Console.WriteLine($"Background task '{LocationUpdateService}' time remaining: {UIApplication.SharedApplication.BackgroundTimeRemaining}");
					await Task.Delay(10000);
				}
			});
		}

		private async void Action()
		{
			Location location = null;
			try
			{
				var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
				CancellationTokenSource source = new CancellationTokenSource();
				CancellationToken cancelToken = source.Token;
				location = await Geolocation.GetLocationAsync(request, cancelToken);
			}
			catch (FeatureNotSupportedException fnsEx)
			{
				// Handle not supported on device exception
			}
			catch (PermissionException pEx)
			{
				// Handle permission exception
			}
			catch (Exception ex)
			{
				// Unable to get location
			}

			if (location == null || App.User == null)
			{
				return;
			}

			var service = new LocationService();
			await service.UpdateCurrentLocationTask(new Models.Location(location.Longitude, location.Latitude), App.User.UserToken);
		}

		/// <summary>
		/// Останавливает сервис.
		/// </summary>
		public void StopService()
		{ }
	}
}
