using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(BidServiceDroid))]
namespace itmit.asb.app.Droid.Services
{
	public class BidServiceDroid : IBidsService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Адрес для получения и создания тревог.
		/// </summary>
		private const string BidApiUri = "http://lk.asb-security.ru/api/bid";

		/// <summary>
		/// Адрес для смены статуса тревоги.
		/// </summary>
		private const string ChangeStatusUri = "http://lk.asb-security.ru/api/bid/changeStatus";
		#endregion

		#region Fields
		/// <summary>
		/// Токен пользователя.
		/// </summary>
		public UserToken Token
		{
			get;
			set;
		}
		#endregion
		#endregion

		#region .ctor
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="BidsService" />.
		/// </summary>
		public BidServiceDroid()
		{

		}
		#endregion

		#region IBidsService members
		/// <summary>
		/// Создает при помощи API новую тревогу.
		/// </summary>
		/// <param name="bid">Тревога, которую необходимо сохранить на сервере.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public async Task<bool> CreateBid(Bid bid)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token.TokenType, Token.Token);
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
				var response = await client.PostAsync(BidApiUri, new FormUrlEncodedContent(data));

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (!response.IsSuccessStatusCode)
				{
					LastError = JsonConvert.DeserializeObject<JsonDataResponse<string>>(jsonString).Data;
				}

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Получает тревоги в соответствии с заданным статусом.
		/// </summary>
		/// <param name="status">Статус получаемых тревог.</param>
		/// <returns>Тревоги.</returns>
		public async Task<IEnumerable<Bid>> GetBidsAsync(BidStatus status)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{Token.TokenType} {Token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				response = await client.GetAsync(BidApiUri + $"?status={status.ToString()}");
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Bid>>>(jsonString);

					foreach (var bid in jsonData.Data)
					{
						bid.Client.ImageSource = "http://lk.asb-security.ru/" + bid.Client.ImageSource;
					}

					return await Task.FromResult(jsonData.Data);
				}
			}

			return await Task.FromResult(new List<Bid>());
		}

		/// <summary>
		/// Получает все тревоги.
		/// </summary>
		/// <returns>Тревоги.</returns>
		public async Task<IEnumerable<Bid>> GetBidsAsync()
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{Token.TokenType} {Token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = await client.GetAsync(BidApiUri);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<List<Bid>>>(jsonString);

						foreach (var bid in jsonData.Data)
						{
							bid.Client.ImageSource = "http://lk.asb-security.ru/" + bid.Client.ImageSource;
						}

						return await Task.FromResult(jsonData.Data);
					}
				}
				return await Task.FromResult(new List<Bid>());
			}
		}

		/// <summary>
		/// Устанавливает новый статус у тревоги.
		/// </summary>
		/// <param name="bid">Заявка у которой необходимо установить новый статус.</param>
		/// <param name="status">Новый статус тервоги.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public async Task<bool> SetBidStatusAsync(Bid bid, BidStatus status)
		{
			using (var client = new HttpClient())
			{
				HttpResponseMessage response;
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{Token.TokenType} {Token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				response = await client.PostAsync(ChangeStatusUri,
												  new FormUrlEncodedContent(new Dictionary<string, string>
												  {
													  {
														  "uid", bid.Guid.ToString()
													  },
													  {
														  "new_status", status.ToString()
													  }
												  }));
#if DEBUG
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
#endif

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		/// <summary>
		/// Синхронизирует текущее местоположение тревоги с сервером.
		/// </summary>
		/// <param name="bid">Синхронизируемая тревога.</param>
		/// <returns>Возвращает <c>true</c> в случае успеха, иначе <c>false</c>.</returns>
		public async Task<Bid> SyncBidLocation(Bid bid)
		{
			using (var client = new HttpClient(new CustomAndroidClientHandler()))
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{Token.TokenType} {Token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = await client.PostAsync(SyncLocationUri,
																	  new FormUrlEncodedContent(new Dictionary<string, string>
																	  {
																		  {
																			  "uid", bid.Guid.ToString()
																		  }
																	  }));

				string jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<Bid>>(jsonString);

						bid.Location = jsonData.Data.Location;
						bid.UpdatedAt = jsonData.Data.UpdatedAt;
					}
				}
				return await Task.FromResult(bid);
			}
		}

		public string LastError
		{
			get;
			private set;
		}

		public string SyncLocationUri = "http://lk.asb-security.ru/api/bid/updateCoordinates";
		#endregion
	}
}
