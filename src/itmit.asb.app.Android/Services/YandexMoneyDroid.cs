using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using Newtonsoft.Json;
using Xamarin.Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(YandexMoneyDroid))]
namespace itmit.asb.app.Droid.Services
{
	public class YandexMoneyDroid : IYandexCheckout
	{
		private const string CreatePaymentUri = "http://lk.asb-security.ru/api/client/setActivityFrom";
		private const string CapturePaymentUri = "http://lk.asb-security.ru/api/client/capturePayment";

		public void Buy()
		{
			MainActivity.Instance.InitUi();
			MainActivity.Instance.CheckoutService = this;
		}

		public async Task<Uri> CreatePayment(string paymentToken, UserToken userToken)
		{
			using (var client = new HttpClient(new AndroidClientHandler()))
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{userToken.TokenType} {userToken.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var data = new Dictionary<string, string>
				{
					{
						"payment_token", paymentToken
					}
				};

				var response = await client.PostAsync(CreatePaymentUri, new FormUrlEncodedContent(data));
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
				if (response.IsSuccessStatusCode)
				{
					if (!string.IsNullOrEmpty(jsonString))
					{
						return new Uri(jsonString);
					}
				}

				return null;
			}
		}

		public async Task<Payment> CapturePayment(string paymentToken, UserToken userToken)
		{
			using (var client = new HttpClient(new AndroidClientHandler()))
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{userToken.TokenType} {userToken.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var data = new Dictionary<string, string>
				{
					{
						"payment_token", paymentToken
					}
				};

				var response = await client.PostAsync(CapturePaymentUri, new FormUrlEncodedContent(data));
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<Payment>>(jsonString);
						return await Task.FromResult(jsonData.Data);
					}
				}
			}

			return null;
		}
	}
}
