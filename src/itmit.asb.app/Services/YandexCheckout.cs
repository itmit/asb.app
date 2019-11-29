using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using Xamarin.Forms;

namespace itmit.asb.app.Services
{
	public class YandexCheckout : IYandexCheckout
	{
		private const string PaymentUri = "http://lk.asb-security.ru/api/payment";
		private const string PaymentSuccessUri = "lk.asb-security.ru/api/paymentSuccess";

		public async void Buy()
		{
			using (var client = new HttpClient())
			{
				var user = App.User;
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.UserToken.TokenType} {user.UserToken.Token}");

				var response = await client.GetAsync(PaymentUri);

				var html = await response.Content.ReadAsStringAsync();
				var page = new ContentPage
				{
					Title = "YandexCheckout"
				};
				var view = new WebView
				{
					Source = new HtmlWebViewSource
					{
						Html = html
					}
				};
				view.Navigated += ViewOnNavigated;
				page.Content = view;

				await Application.Current.MainPage.Navigation.PushModalAsync(page);
			}
		}

		public Task<string> GetPaymentStatus(string paymentToken, UserToken userToken) => throw new NotImplementedException();

		public Task<bool> Activate(string paymentToken, UserToken userToken) => throw new NotImplementedException();

		private void ViewOnNavigated(object sender, WebNavigatedEventArgs e)
		{
			if (e.Url.Contains(PaymentSuccessUri))
			{
				Application.Current.MainPage.Navigation.PopModalAsync();
			}
		}

		public Task<Uri> CreatePayment(string paymentToken, UserToken userToken) => throw new NotImplementedException();

		public Task<Payment> CapturePayment(string paymentToken, UserToken userToken) => throw new NotImplementedException();
	}
}
