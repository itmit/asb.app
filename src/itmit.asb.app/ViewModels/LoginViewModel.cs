using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using itmit.asb.app.Views.Guard;
using Newtonsoft.Json;
using Realms;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		private string _login;
		private string _password;
		private bool _authNotSuccess;
		private readonly IAuthService _authService = DependencyService.Get<IAuthService>();

		private Realm Realm => Realm.GetInstance();

		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(obj =>
			{
				LoginCommandExecute();
			},
			obj => App.User == null && Login != string.Empty && Password != string.Empty);

			AuthNotSuccess = false;
		}

		public bool AuthNotSuccess
		{
			get => _authNotSuccess;
			set => SetProperty(ref _authNotSuccess, value);
		}

		public RelayCommand LoginCommand
		{
			get;
			set;
		}

		private async void LoginCommandExecute()
		{
			User user;
			try
			{
				user = await _authService.GetUserByTokenAsync(await _authService.LoginAsync(Login, Password));
			}
			catch(AuthenticationException e)
			{
				AuthNotSuccess = true;
				Debug.WriteLine(e);
				return;
			}

			Realm.Write(() =>
			{
				Realm.Add(user, true);
			});

			if (user.IsGuard)
			{
				Application.Current.MainPage = new NavigationPage(new GuardMainPage());
				return;
			}

			Application.Current.MainPage = new MainPage();
			
		}

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		public string Login
		{
			get => _login;
			set => SetProperty(ref _login, value);
		}
	}
}
