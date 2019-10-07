
﻿using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Realms;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		private INavigation _navigation;
		private string _activeTo;
		private bool _isShowedIndicator;
		private bool _isShowedActivityTitle = true;

		public AboutViewModel(INavigation navigation)
		{
			Instance = this;
			var user = App.User;
			if (user != null)
			{
				IsShowedActivityTitle = user.ActiveFrom.Ticks > DateTime.MinValue.Ticks;
				if (IsShowedActivityTitle)
				{
					ActiveTo = user.ActiveFrom.DateTime.Add(new TimeSpan(30, 3, 0, 0))
								   .ToString("dd.MM.yyyy hh:mm");
				}
				else
				{
					ActiveTo = "Не активна";
				}
			}

			_navigation = navigation;
			OpenRobokassa = new RelayCommand(obj =>
			{
				DependencyService.Get<IYandexCheckout>().Buy();

				
			}, obj => true);
		}

		public static AboutViewModel Instance
		{
            using(var client = new HttpClient())
            {


                var response = await client.GetAsync("https://auth.robokassa.ru/Merchant/Index.aspx?MerchantLogin=demo&OutSum=11.00&InvId=&Description=%D0%9E%D0%BF%D0%BB%D0%B0%D1%82%D0%B0%20%D0%B7%D0%B0%D0%BA%D0%B0%D0%B7%D0%B0%20%D0%B2%20%D0%A2%D0%B5%D1%81%D1%82%D0%BE%D0%B2%D0%BE%D0%BC%20%D0%BC%D0%B0%D0%B3%D0%B0%D0%B7%D0%B8%D0%BD%D0%B5%20ROBOKASSA&shp_Item=1&Culture=ru&Encoding=utf-8&Receipt=%7B%22sno%22%3A%22osn%22%2C%22items%22%3A%5B%7B%22name%22%3A%22%D0%A2%D0%B5%D1%85%D0%BD%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D0%B4%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F%20%D0%BF%D0%BE%20ROBOKASSA%22%2C%22quantity%22%3A1.0%2C%22sum%22%3A6.0%2C%22tax%22%3A%22vat18%22%7D%2C%7B%22name%22%3A%22%D0%A2%D0%B5%D1%85%D0%BD%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D0%B4%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F%20%D0%BF%D0%BE%20Robo.market%22%2C%22quantity%22%3A1.0%2C%22sum%22%3A5.0%2C%22tax%22%3A%22vat18%22%7D%5D%7D&SignatureValue=3925b771e47d405cbcbb492daa936824");
                var str = await response.Content.ReadAsStringAsync();
                var view = new WebView
                {
                    Margin = 10,
                    Source = str
                };
                var page = new ContentPage()
                {
                    Title = "ROBOKASSA",
                    Content = view
                };
                await _navigation.PushAsync(page);
            }

			get;
			private set;
		}

		public bool IsShowedActivityTitle
		{
			get => _isShowedActivityTitle;
			set => SetProperty(ref _isShowedActivityTitle, value);
		}

		public bool IsShowedIndicator
		{
			get => _isShowedIndicator;
			set => SetProperty(ref _isShowedIndicator, value);
		}

		public string ActiveTo
		{
			get => _activeTo;
			set => SetProperty(ref _activeTo, value);
		}

		private async void ViewOnNavigating(object sender, WebNavigatingEventArgs e)
		{
			var uri = new Uri(e.Url);
			var path = uri.Host + uri.LocalPath;
			if (path == "www.asb-security.ru/")
			{
				Application.Current.MainPage = new NavigationPage(new AlarmPage());

				var con = RealmConfiguration.DefaultConfiguration;
				con.SchemaVersion = 7;
				Realm.GetInstance(con).Write(() =>
				{
					App.User.IsActive = true;
				});

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
