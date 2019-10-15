using System;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using UIKit;
using Xamarin.Essentials;

namespace itmit.asb.app.iOS.Services
{
	public class LocationServiceIOS : ILocationUpdatesService
	{
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";

		/// <summary>
		/// Запускает сервис.
		/// </summary>
		public async void StartService()
		{
			nint taskId = 0;
			var taskEnded = false;
			taskId = UIApplication.SharedApplication.BeginBackgroundTask(LocationUpdateService, () =>
			{
				//when time is up and task has not finished, call this method to finish the task to prevent the app from being terminated
				Console.WriteLine($"Background task '{LocationUpdateService}' got killed");
				taskEnded = true;
				UIApplication.SharedApplication.EndBackgroundTask(taskId);
			});
			await Task.Factory.StartNew(async () =>
			{
				//here we run the actual task
				Console.WriteLine($"Background task '{LocationUpdateService}' started");
				Action();
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
					await Task.Delay(1000);
				}
			});
		}

		private async void Action()
		{
			var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));

			if (location == null)
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
		{
			throw new System.NotImplementedException();
		}
	}
}
