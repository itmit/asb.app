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
	public class BidsService : IBidsService
	{
		private readonly UserToken _token;
		private const string ItemsListUri = "http://asb.itmit-studio.ru/api/bids";

		public BidsService(UserToken token)
		{
			_token = token;
		}

		public async Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				response = await client.GetAsync(new Uri(ItemsListUri + $"?status={status}"));
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

		public Task<bool> SetBidStatusAsync(Bid bid, BidStatus status) => throw new NotImplementedException();
	}
}
