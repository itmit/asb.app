using System;
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
