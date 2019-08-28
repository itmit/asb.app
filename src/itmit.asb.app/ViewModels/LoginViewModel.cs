using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using itmit.asb.app.Views.Guard;
using Plugin.Permissions.Abstractions;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		#region Data
		#region Fields
		private bool _authNotSuccess;
		private readonly IAuthService _authService = DependencyService.Get<IAuthService>();
		private string _login;
		private string _password;
		#endregion
		#endregion

		#region .ctor
		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(obj =>
											{
												Task.Run(LoginCommandExecute);
											},
											obj => CanLoginCommandExecute());

			AuthNotSuccess = false;
		}
		#endregion

		#region Properties
		public RelayCommand LoginCommand
		{
			get;
			set;
		}

		public bool AuthNotSuccess
		{
			get => _authNotSuccess;
			set => SetProperty(ref _authNotSuccess, value);
		}

		public string Login
		{
			get => _login;
			set => SetProperty(ref _login, value);
		}

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		private Realm Realm => Realm.GetInstance();
		#endregion

		#region Private
		private bool CanLoginCommandExecute() => Connectivity.NetworkAccess == NetworkAccess.Internet && App.User == null && Login != string.Empty && Password != string.Empty;

		private async void LoginCommandExecute()
		{
			await CheckPermission(Permission.Location, "Для отслеживания вашего местоположения необходимо разрешение на использование геоданных.");

			User user;
			try
			{
				user = await _authService.GetUserByTokenAsync(await _authService.LoginAsync(Login, Password));
			}
			catch (AuthenticationException e)
			{
				AuthNotSuccess = true;
				Debug.WriteLine(e);
				return;
			}

			using (var transaction = Realm.BeginWrite())
			{
				Realm.RemoveAll<User>();
				transaction.Commit();
			}

			var app = Application.Current as App;

			if (app == null)
			{
				return;
			}

			Realm.Write(() =>
			{
				Realm.Add(user, true);
			});

			if (user.IsGuard)
			{
				app.MainPage = new GuardMainPage();
				return;
			}

			app.StartBackgroundService(new TimeSpan(0, 0, 0, 5));
			app.MainPage = new AlarmPage();
		}
		#endregion
	}
}
