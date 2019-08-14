using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using Newtonsoft.Json;

namespace itmit.asb.app.Services
{
	public class BidsService : IBidsService
	{
		#region Data
		#region Consts
		private const string BidApiUri = "http://asb.itmit-studio.ru/api/bid";
		private const string ChangeStatusUri = "http://asb.itmit-studio.ru/api/bid/changeStatus";
		#endregion

		#region Fields
		private readonly UserToken _token;
		#endregion
		#endregion

		#region .ctor
		public BidsService(UserToken token) => _token = token;
		#endregion

		#region IBidsService members
		public async void CreateBid(Bid bid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_token.TokenType, _token.Token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var data = new Dictionary<string, string>
				{
					{
						"uid", bid.Guid.ToString()
					},
					{
						"type", bid.Type.ToString()
					},
					{
						"latitude", bid.Location.Latitude.ToString(CultureInfo.InvariantCulture)
					},
					{
						"longitude", bid.Location.Longitude.ToString(CultureInfo.InvariantCulture)
					}
				};
				var response = await client.PostAsync(new Uri(BidApiUri),
													  new FormUrlEncodedContent(data));
#if DEBUG
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
#endif
			}
		}

		public async Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				response = await client.GetAsync(new Uri(BidApiUri + $"?status={status.ToString()}"));
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Bid>>>(jsonString);

					foreach (Bid bid in jsonData.Data)
					{
						bid.Client.UserPictureSource = "http://asb.itmit-studio.ru/" + bid.Client.UserPictureSource;
					}

					return await Task.FromResult(jsonData.Data);
				}
			}

			return await Task.FromResult(new List<Bid>());
		}

		public async Task<IEnumerable<Bid>> GetBidsAsync()
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				response = await client.GetAsync(new Uri(BidApiUri));
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Bid>>>(jsonString);

					foreach (Bid bid in jsonData.Data)
					{
						bid.Client.UserPictureSource = "http://asb.itmit-studio.ru/" + bid.Client.UserPictureSource;
					}

					return await Task.FromResult(jsonData.Data);
				}
			}

			return await Task.FromResult(new List<Bid>());
		}
		public async void SetBidStatusAsync(Bid bid, BidStatus status)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{_token.TokenType} {_token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				response = await client.PostAsync(new Uri(ChangeStatusUri),
												  new FormUrlEncodedContent(new Dictionary<string, string>
												  {
													  {
														  "uid", bid.Guid.ToString()
													  },
													  {
														  "new_status", status.ToString()
													  }
												  }));
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				return;
			}

			throw new AuthenticationException($"Пользователь с таким токеном, не найден. Токен: {_token.Token}");
		}
		#endregion
	}
}
