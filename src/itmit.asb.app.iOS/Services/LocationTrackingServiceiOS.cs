using System;
using CoreLocation;
using itmit.asb.app.iOS.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = itmit.asb.app.Models.Location;

[assembly: Dependency(typeof(LocationTrackingServiceIos))]
namespace itmit.asb.app.iOS.Services
{
	public class LocationTrackingServiceIos : ILocationTrackingService
	{
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";

		public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return;
			}

			var token = App.User.UserToken;
			if (token == null)
			{
				return;
			}

			try
			{
				UpdateLocation(token, e);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		private async void UpdateLocation(UserToken token, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			CLLocation location = e.Location;

			var service = new LocationService();
			if (App.User.IsGuard)
			{
				await service.UpdateCurrentLocationTask(new Location(location.Coordinate.Longitude, location.Coordinate.Latitude), token);
			}
			else
			{
				await service.AddPointOnMapTask(
					new Location(location.Coordinate.Latitude, location.Coordinate.Longitude), token);
			}
		}

		public static LocationManager Manager { get; set; }

		/// <summary>
		/// Запускает сервис для отслеживания.
		/// </summary>
		/// <param name="bidGuid">Ид тревоги.</param>
		public void StartService(Guid bidGuid)
		{
			Manager = new LocationManager();
			Manager.LocationUpdated += HandleLocationChanged;
			Manager.StartLocationUpdates();
		}
	}
}
