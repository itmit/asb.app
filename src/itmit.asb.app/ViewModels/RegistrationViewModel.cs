using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Realms;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
    /// <summary>
    /// Представляет модель представления для регистрации.
    /// </summary>
    public class RegistrationViewModel : BaseViewModel
	{
		private string _phoneNumber;
		private string _userType;
		private string _password;
		private string _confirmPassword;
		private bool _isEntity;
		private bool _isIndividual;
		private string _name;
		private string _organization;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="RegistrationViewModel" />.
		/// </summary>
		public RegistrationViewModel()
		{
			Register = new RelayCommand(obj =>
			{
				if (string.IsNullOrEmpty(Password)
					|| string.IsNullOrEmpty(ConfirmPassword)
					|| string.IsNullOrEmpty(PhoneNumber)
					|| UserType.Equals("Юридическое") && string.IsNullOrEmpty(Organization)
					|| UserType.Equals("Физическое") && string.IsNullOrEmpty(Name))
				{
					Application.Current.MainPage.DisplayAlert("Уведомление", "Пожалуйста, внимательно заполните все поля формы.", "ОK");
					return;
				}

				var user = new User
				{
					PhoneNumber = PhoneNumber
				};

				if (UserType.Equals("Юридическое"))
				{
					user.Organization = Organization;
					user.UserType = Models.UserType.Entity;
				}
				else if (UserType.Equals("Физическое"))
				{
					user.Name = Name;
					user.UserType = Models.UserType.Individual;
				}

				OnRegister(user, Password, ConfirmPassword);
				
			}, obj => !IsBusy && Connectivity.NetworkAccess == NetworkAccess.Internet);
		}

		public bool IsEntity
		{
			get => _isEntity;
			set
			{
				SetProperty(ref _isEntity, value);
				SetProperty(ref _isIndividual, !value, nameof(IsIndividual));
			}
		}

		public bool IsIndividual
		{
			get => _isIndividual;
			set
			{
				SetProperty(ref _isIndividual, value);
				SetProperty(ref _isEntity, !value, nameof(IsEntity));
			}
		}

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Organization
		{
			get => _organization;
			set => SetProperty(ref _organization, value);
		}

		/// <summary>
		/// Возвращает или устанавливает телефонный номер.
		/// </summary>
		public string PhoneNumber
		{
			get => _phoneNumber;
			set => SetProperty(ref _phoneNumber, value);
		}

		/// <summary>
		/// Возвращает или устанавливает тип пользователя.
		/// </summary>
		public string UserType
		{
			get => _userType;
			set
			{
				if (value.Equals("Юридическое"))
				{
					IsEntity = true;
				}
				else if (value.Equals("Физическое"))
				{
					IsEntity = false;
				}
				SetProperty(ref _userType, value);
			}
		}

		/// <summary>
		/// Возвращает или устанавливает пароль пользователя.
		/// </summary>
		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		/// <summary>
		/// Возвращает или устанавливает подтвержденный пароль пользователя.
		/// </summary>
		public string ConfirmPassword
		{
			get => _confirmPassword;
			set => SetProperty(ref _confirmPassword, value);
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
			if (!pass.Equals(cPass))
			{
				await Application.Current.MainPage.DisplayAlert("Уведомление", "Пароли не совпадают.", "ОK");
				return;
			}

			var app = Application.Current as App;

			if (app == null)
			{
				return;
			}

			IAuthService service = new AuthService();
			var token = await service.RegisterAsync(user, pass, cPass);
			if (!string.IsNullOrEmpty(token.Token))
			{
				app.StartBackgroundService(new TimeSpan(0, 0, 0, 5));

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
