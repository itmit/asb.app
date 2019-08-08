using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using itmit.asb.app.Models;
using Newtonsoft.Json;

namespace itmit.asb.app.Services
{
	/// <summary>
	/// Представляет сервис для авторизации.
	/// </summary>
	public class AuthService : IAuthService
	{
		#region Data
		#region Consts
		/// <summary>
		/// Задает адрес авторизации.
		/// </summary>
		private const string AuthUri = "http://asb.itmit-studio.ru/api/login";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string DetailsUri = "http://asb.itmit-studio.ru/api/details";

		/// <summary>
		/// Задает ключ к api для авторизации.
		/// </summary>
		private const string SecretKey = "znrAr76W8rN22aMAcAT0BbYFcF4ivR8j9GVAOgkD";
		#endregion
		#endregion

		public AuthService()
		{ }

		#region Public
		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token">Токен для получения пользователя</param>
		/// <returns>Авторизованный пользователь.</returns>
		public async Task<User> GetUserByTokenAsync(UserToken token)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");

				response = await client.PostAsync(new Uri(DetailsUri), null);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
					jsonData.Data.UserToken = token;
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthenticationException($"Пользователь с таким токеном, не найден. Токен: {token.Token}");
		}

		/// <summary>
		/// Отправляет запрос на авторизацию, по api.
		/// </summary>
		/// <param name="login">Логин для авторизации.</param>
		/// <param name="pass">Пароль для авторизации.</param>
		/// <returns>Токен авторизованного пользователя.</returns>
		/// <exception cref="AuthenticationException">Возникает при неудачной авторизации.</exception>
		public async Task<UserToken> LoginAsync(string login, string pass)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"phoneNumber", login
					},
					{
						"password", pass
					}
				});

				response = await client.PostAsync(new Uri(AuthUri), encodedContent);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<UserToken>>(jsonString);
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthenticationException($"Возникла ошибка при авторизации. Логин: {login}");
		}
		#endregion
	}
}
