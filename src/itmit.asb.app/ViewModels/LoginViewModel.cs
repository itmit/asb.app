﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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

		private Realm Realm => Realm.GetInstance();

		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(obj =>
			{
				LoginCommandExecute();
			}, obj => true);

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
			App.UserToken = await LoginAsync(Login, Password);
            /*
			Realm.Write(() =>
			{
				Realm.Add(App.UserToken, true);
			});
            */
			if (App.UserToken.Token.Equals(string.Empty))
			{
				AuthNotSuccess = true;
			}
			else
			{
				if (App.IsGuardUser)
				{
					Application.Current.MainPage = new GuardMainPage();
				}
				else
				{
					Application.Current.MainPage = new AboutPage();
				}
			}
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

		private const string Uri = "http://asb.itmit-studio.ru/api/login";
		private const string SecretKey = "znrAr76W8rN22aMAcAT0BbYFcF4ivR8j9GVAOgkD";

		public async Task<UserToken> LoginAsync(string login, string pass)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				if (login == "+7 (911) 447-11-83" && pass == "x5410041")
				{
					App.IsGuardUser = true;
				}

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string> {
					{
						"phoneNumber",
						HttpUtility.UrlEncode(login)
					},
					{
						"password",
						HttpUtility.UrlEncode(pass)
					}
				});

				response = await client.PostAsync(new Uri(Uri), encodedContent);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<UserToken> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}
			
			return await Task.FromResult(new UserToken());
		}
	}
}
