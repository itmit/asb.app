using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public class LocationDataStore : IDataStore<Location>
	{
		private const string Uri = "http://asb.itmit-studio.ru/api/bid";

		public async Task<bool> AddItemAsync(Location item)
		{
			var client = new HttpClient();

			var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string> {
				{
					"latitude",
					item.Latitude.ToString(CultureInfo.InvariantCulture)
				},
				{
					"longitude",
					item.Longitude.ToString(CultureInfo.InvariantCulture)
				}
			});

			var response = await client.PostAsync(new Uri(Uri), encodedContent);

			if (!response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
			}

			return await Task.FromResult(true);
		}

		public Task<bool> UpdateItemAsync(Location item) => throw new System.NotImplementedException();

		public Task<bool> DeleteItemAsync(string id) => throw new System.NotImplementedException();

		public Task<Location> GetItemAsync(string id) => throw new System.NotImplementedException();

		public Task<IEnumerable<Location>> GetItemsAsync(bool forceRefresh = false) => throw new System.NotImplementedException();
	}
}
