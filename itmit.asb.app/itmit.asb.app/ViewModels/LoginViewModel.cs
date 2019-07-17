using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		private string _login;
		private string _password;

		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(obj =>
			{
				LoginCommandExecute();
			}, obj => true);
		}

		public RelayCommand LoginCommand
		{
			get;
			set;
		}

		private async void LoginCommandExecute()
		{
			App.User = await LoginAsync(Login, Password);
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

		private const string Uri = "http://asb.itmit-studio.ru/api/login";
		private const string SecretKey = "2QAHVdLTLmODi0gFAmr8vScFLGLVIGam0Y6bDdh9";

		public async Task<User> LoginAsync(string login, string pass)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string> {
					{
						"email",
						login
					},
					{
						"password",
						pass
					}
				});

				response = await client.PostAsync(new Uri(Uri), encodedContent);
			}

			if (response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
				if (jsonString != null)
				{
					JsonDataResponse<User> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}
			
			return await Task.FromResult(new User());
		}
	}
}
