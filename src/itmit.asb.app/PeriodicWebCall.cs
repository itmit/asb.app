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
		#region Data
		#region Fields
		private readonly ILocationService _locationService = new LocationService();
		#endregion
		#endregion

		#region .ctor
		public PeriodicWebCall(TimeSpan interval) => Interval = interval;
		#endregion

		#region IPeriodicTask members
		public TimeSpan Interval
		{
			get;
		}

		public async Task<bool> StartJob()
		{
			Debug.WriteLine($"Update location at {DateTime.Now};");

			return await _locationService.UpdateCurrentLocationTask(await Location.GetCurrentGeolocationAsync(GeolocationAccuracy.Medium), App.User.UserToken);
		}
		#endregion
	}
}
