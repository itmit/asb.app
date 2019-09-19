using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		private INavigation _navigation;

		public AboutViewModel(INavigation navigation)
		{
			_navigation = navigation;
			OpenRobokassa = new RelayCommand(obj =>
			{
				OpenRobokassaExecute();
			}, obj => true);
		}

		private async void OpenRobokassaExecute()
		{
			var view = new WebView
			{
				Margin = 10,
				Source = "https://auth.robokassa.ru/Merchant/Index.aspx?MerchantLogin=asbapp&OutSum=1.00&InvoiceID=1&Description=%D0%A2%D0%BE%D0%B2%D0%B0%D1%80%D1%8B%20%D0%B4%D0%BB%D1%8F%20%D0%B6%D0%B8%D0%B2%D0%BE%D1%82%D0%BD%D1%8B%D1%85&SignatureValue=9f314f76aaf59f0d555af5c6f396b0ff&IsTest=1"
			};
			view.Navigating += ViewOnNavigating;
			var page = new ContentPage()
			{
				Title = "ROBOKASSA",
				Content = view
			};
			await _navigation.PushAsync(page);
		}

		private async void ViewOnNavigating(object sender, WebNavigatingEventArgs e)
		{
			var uri = new Uri(e.Url);
			var path = uri.Host + uri.LocalPath;
			if (path == "www.asb-security.ru/")
			{
				Application.Current.MainPage = new NavigationPage(new AlarmPage());
				IAuthService service = new AuthService();
				await service.SetActivityFrom(App.User.UserToken);
				await Task.Delay(150);
				await Application.Current.MainPage.DisplayAlert("Внимание", "Подписка оплачена.", "Ок");
			}
			else if (path == "www.asb-security.ru/structure/")
			{
				Application.Current.MainPage = new NavigationPage(new AboutPage());
				await Task.Delay(150);
				await Application.Current.MainPage.DisplayAlert("Внимание", "Не удалось оплатить подписку.", "Ок");
			}

		}

		public ICommand OpenRobokassa
		{
			get;
		}
	}
}
