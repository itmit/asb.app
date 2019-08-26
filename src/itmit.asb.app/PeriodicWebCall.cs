using System;
using System.Diagnostics;
using System.Threading.Tasks;
using itmit.asb.app.Services;
using Matcha.BackgroundService;
using Xamarin.Essentials;
using Location = itmit.asb.app.Models.Location;

namespace itmit.asb.app
{
	public class PeriodicWebCall : IPeriodicTask
	{
		private readonly ILocationService _locationService = new LocationService();

		public PeriodicWebCall(TimeSpan interval)
		{
			Interval = interval;
		}

		public async Task<bool> StartJob()
		{
			Debug.WriteLine($"Update location at {DateTime.Now};");

			return await _locationService.UpdateCurrentLocationTask(
				await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Medium),
				App.User.UserToken
			);
		}

		public TimeSpan Interval
		{
			get;
		}
	}
}
