using System;
using System.Threading;
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
			Xamarin.Essentials.Location location = null;
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
