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
	public class LocationDataStore : ILocationService
	{
		#region Data
		#region Consts
		private const string Uri = "http://asb.itmit-studio.ru/api/bid";

		private const string UpdateCurrentLocationUri = "http://asb.itmit-studio.ru/api/client/updateCurrentLocation"; 
		#endregion
		#endregion

		public async Task<bool> AddPointOnMapTask(Location location, UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.Token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"latitude", location.Latitude.ToString(CultureInfo.InvariantCulture)
					},
					{
						"longitude", location.Longitude.ToString(CultureInfo.InvariantCulture)
					}
				});

				var response = await client.PostAsync(new Uri(Uri), encodedContent);
#if DEBUG
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
#endif
				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		public async Task<bool> UpdateCurrentLocationTask(Location location, UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.Token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync(
								   UpdateCurrentLocationUri, 
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

				bool result = await Task.FromResult(response.IsSuccessStatusCode);

				if (!result)
				{
					LastError = jsonString;
				}

				return result;
			}
		}

		public string LastError
		{
			get;
			private set;
		}
	}
}
