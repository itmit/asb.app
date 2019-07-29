using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using Newtonsoft.Json;

namespace itmit.asb.app.Services
{
	public class BidsDataStore : IDataStore<Bid>
	{
		private readonly UserToken _token;
		private const string ItemsListUri = "http://asb.itmit-studio.ru/api/bids";

		public BidsDataStore(UserToken token)
		{
			_token = token;
		}

		public Task<bool> AddItemAsync(Bid item) => throw new System.NotImplementedException();

		public Task<bool> UpdateItemAsync(Bid item) => throw new System.NotImplementedException();

		public Task<bool> DeleteItemAsync(string id) => throw new System.NotImplementedException();

		public Task<Bid> GetItemAsync(string id) => throw new System.NotImplementedException();

		public async Task<IEnumerable<Bid>> GetItemsAsync(bool forceRefresh = false)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				response = await client.PostAsync(new Uri(ItemsListUri), null);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<List<Bid>> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Bid>>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}

			return await Task.FromResult(new List<Bid>());
		}
	}
}
