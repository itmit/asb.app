using System.Threading.Tasks;
using Xamarin.Essentials;

namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет место клиента.
	/// </summary>
	public class Location
	{
		#region Data
		#region Static
		
		/// <summary>
		/// Последнее сохраненное место клиента. 
		/// </summary>
		private static Location _currentGeolocation;
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новую позицию с координатами.
		/// </summary>
		/// <param name="latitude">Широта.</param>
		/// <param name="longitude">Долгота.</param>
		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает широту.
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает долготу.
		/// </summary>
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
