using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	/// <summary>
	/// Представляет механизм для работы с местоположением.
	/// </summary>
	public class LocationService : ILocationService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Адрес для API создания точки на карте.
		/// </summary>
		private const string CreatePointUri = "http://lk.asb-security.ru/api/pointOnMap";

		/// <summary>
		/// Адрес для API обновления местоположения пользователя.
		/// </summary>
		private const string UpdateCurrentLocationUri = "http://lk.asb-security.ru/api/client/updateCurrentLocation";
		#endregion
		#endregion

		#region ILocationService members
		/// <summary>
		/// Создает через API точку на карте.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя для авторизации.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public Task<bool> AddPointOnMapTask(Location location, UserToken token) => AddPointOnMapTask(location, token, Guid.Empty);

		/// <summary>
		/// Создает через API точку на карте, и привязывает его к тревоге.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя для авторизации.</param>
		/// <param name="guid">Ид тревоги, местоположение которой необходимо изменить.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public async Task<bool> AddPointOnMapTask(Location location, UserToken token, Guid guid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new Dictionary<string, string>
				{
					{
						"latitude", location.Latitude.ToString(CultureInfo.InvariantCulture)
					},
					{
						"longitude", location.Longitude.ToString(CultureInfo.InvariantCulture)
					}
				};

				if (guid != Guid.Empty)
				{
					encodedContent.Add("uid", guid.ToString());
				}

				var response = await client.PostAsync(CreatePointUri, new FormUrlEncodedContent(encodedContent));
#if DEBUG
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
#endif
				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Возвращает последнюю ошибку, которая вернулась из API.
		/// </summary>
		public string LastError
		{
			get;
			private set;
		}

		/// <summary>
		/// Обновляет текущее местоположение клиента.
		/// </summary>
		/// <param name="location">Местоположение.</param>
		/// <param name="token">Токен пользователя, чье местоположение необходимо обновить.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public async Task<bool> UpdateCurrentLocationTask(Location location, UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(UpdateCurrentLocationUri,
													  new FormUrlEncodedContent(new Dictionary<string, string>
													  {
														  {
															  "latitude", location.Latitude.ToString(CultureInfo.InvariantCulture)
														  },
														  {
															  "longitude", location.Longitude.ToString(CultureInfo.InvariantCulture)
														  }
													  }));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				var result = await Task.FromResult(response.IsSuccessStatusCode);

				if (!result)
				{
					LastError = jsonString;
				}

				return result;
			}
		}
		#endregion
	}
}
