using System.Threading.Tasks;
using Xamarin.Essentials;

namespace itmit.asb.app.Models
{
	public class Location
	{
		private static Location _currentGeolocation;

		public Location (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

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

		/// <summary>
		/// Возвращает текущие координаты местоположения.
		/// </summary>
		/// <returns>Текущие координаты.</returns>
		public static async Task<Location> GetCurrentGeolocationAsync(GeolocationAccuracy accuracy)
		{
			var request = new GeolocationRequest(accuracy);
			var location = await Geolocation.GetLocationAsync(request);

			if (location == null) {
				return await Task.FromResult(_currentGeolocation);
			}

			_currentGeolocation = new Location(location.Latitude, location.Longitude);
			return await Task.FromResult(_currentGeolocation);
		}
	}
}
