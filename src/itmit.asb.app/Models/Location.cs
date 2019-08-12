using System.Threading.Tasks;
using Xamarin.Essentials;

namespace itmit.asb.app.Models
{
	public class Location
	{
		#region Data
		#region Static
		private static Location _currentGeolocation;
		#endregion
		#endregion

		#region .ctor
		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
		#endregion

		#region Properties
		public double Latitude
		{
			get;
			set;
		}

		public double Longitude
		{
			get;
			set;
		}
		#endregion

		#region Public
		/// <summary>
		/// Возвращает текущие координаты местоположения.
		/// </summary>
		/// <returns>Текущие координаты.</returns>
		public static async Task<Location> GetCurrentGeolocationAsync(GeolocationAccuracy accuracy)
		{
			var request = new GeolocationRequest(accuracy);
			var location = await Geolocation.GetLocationAsync(request);

			if (location == null)
			{
				return await Task.FromResult(_currentGeolocation);
			}

			_currentGeolocation = new Location(location.Latitude, location.Longitude);
			return await Task.FromResult(_currentGeolocation);
		}
		#endregion
	}
}
