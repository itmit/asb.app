using System;
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
		private DateTime _lastUpdateTime;

		#region Data
		#region Consts
		public const string LocationUpdateService = "LocationUpdateServiceBackgroundTaskManager";
		#endregion
		#endregion

		#region Properties
		public static LocationManager Manager
		{
			get;
			set;
		}
		#endregion

		#region Public
		public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
		{
			if (Connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return;
			}

			if (DateTime.Now.Subtract(_lastUpdateTime) < TimeSpan.FromSeconds(10))
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
				_lastUpdateTime = DateTime.Now;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}
		#endregion

		#region ILocationTrackingService members
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
		#endregion

		#region Private
		private async void UpdateLocation(UserToken token, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			var location = e.Location;

			var service = new LocationService();
			if (App.User.IsGuard)
			{
				await service.UpdateCurrentLocationTask(new Location(location.Coordinate.Longitude, location.Coordinate.Latitude), token);
			}
			else
			{
				await service.AddPointOnMapTask(new Location(location.Coordinate.Latitude, location.Coordinate.Longitude), token);
			}
		}
		#endregion
	}
}
