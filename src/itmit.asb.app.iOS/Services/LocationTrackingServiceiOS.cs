using System;
using CoreLocation;
using itmit.asb.app.Services;

namespace itmit.asb.app.iOS.Services
{
	public class LocationTrackingServiceIos : ILocationTrackingService
	{
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";


		public async void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
		{
			var token = App.User.UserToken;
			if (token == null)
			{
				return;
			}

			// Handle foreground updates
			CLLocation location = e.Location;

			var service = new LocationService();
			await service.UpdateCurrentLocationTask(new Models.Location(location.Coordinate.Longitude, location.Coordinate.Latitude), token);

			Console.WriteLine("foreground updated");
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
