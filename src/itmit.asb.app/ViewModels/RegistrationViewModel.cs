using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	/// <summary>
	/// Представляет модель представления для регистрации.
	/// </summary>
	public class RegistrationViewModel : BaseViewModel
	{
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="RegistrationViewModel" />.
		/// </summary>
		public RegistrationViewModel()
		{
			Register = new RelayCommand(obj =>
			{
				var user = new User()
				{
					PhoneNumber = PhoneNumber
				};

				if (UserType.Equals("Юридическое"))
				{
					user.UserType = Models.UserType.Entity;
				}
				else if (UserType.Equals("Физическое"))
				{
					user.UserType = Models.UserType.Individual;
				}

				OnRegister(user, Password, ConfirmPassword);
				
			}, obj => !IsBusy && Connectivity.NetworkAccess == NetworkAccess.Internet);
		}

		/// <summary>
		/// Возвращает или устанавливает телефонный номер.
		/// </summary>
		public string PhoneNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает тип пользователя.
		/// </summary>
		public string UserType
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает пароль пользователя.
		/// </summary>
		public string Password
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает подтвержденный пароль пользователя.
		/// </summary>
		public string ConfirmPassword
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает команду для регистрации.
		/// </summary>
		public ICommand Register
		{
			get;
		}

		private Realm Realm => Realm.GetInstance();

		private async void OnRegister(User user, string pass, string cPass)
		{
			var app = Application.Current as App;

			if (app == null)
			{
				return;
			}

			IAuthService service = new AuthService();
			var token = await service.RegisterAsync(user, pass, cPass);
			if (!string.IsNullOrEmpty(token.Token))
			{
				app.MainPage = new AlarmPage();

				user.UserToken = token;

				Realm.Write(() =>
				{
					Realm.Add(user, true);
				});
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Уведомление", "Ошибка регистрации", "ОK");
			}
		}
	}
}
