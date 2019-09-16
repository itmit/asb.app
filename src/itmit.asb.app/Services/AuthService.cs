using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
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
		/// Задает адрес для регистрации.
		/// </summary>
		private const string RegisterUri = "http://asb.itmit-studio.ru/api/register";

		/// <summary>
		/// Задает адрес для получения картинок.
		/// </summary>
		private const string BasePictureUri = "http://asb.itmit-studio.ru/";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string DetailsUri = "http://asb.itmit-studio.ru/api/details";

		/// <summary>
		/// Задает ключ к api для авторизации.
		/// </summary>
		private const string SecretKey = "znrAr76W8rN22aMAcAT0BbYFcF4ivR8j9GVAOgkD";

		/// <summary>
		/// Задает адрес для сохранения примечания.
		/// </summary>
		private const string SetNodeUri = "http://asb.itmit-studio.ru/api/client/note";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string UploadImageUrl = "http://asb.itmit-studio.ru/api/client/changePhoto";
		#endregion
		#endregion

		#region Public
		/// <summary>
		/// Устанавливает аватар клиента.
		/// </summary>
		/// <param name="image">Массив байтов картинки отправляемые на сервер.</param>
		/// <param name="token">Токен пользователя.</param>
		public async void SetAvatar(byte[] image, UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var byteArrayContent = new ByteArrayContent(image);
				byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

				var response = await client.PostAsync(UploadImageUrl,
													  new MultipartFormDataContent
													  {
														  {
															  byteArrayContent, "\"contents\"", "\"feedback.jpeg\""
														  }
													  });

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
			}
		}

		/// <summary>
		/// Устанавливает примечание пользователя.
		/// </summary>
		/// <param name="note">Примечание пользователя.</param>
		/// <param name="token">Токен пользователя.</param>
		public async void SetNode(string note, UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"note", note
					}
				});

				var response = await client.PostAsync(new Uri(SetNodeUri), content);

				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);
			}
		}
		#endregion

		#region IAuthService members
		/// <summary>
		/// Получает данные авторизованного пользователя по токену.
		/// </summary>
		/// <param name="token">Токен для получения пользователя.</param>
		/// <returns>Авторизованный пользователь.</returns>
		public async Task<User> GetUserByTokenAsync(UserToken token)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
					jsonData.Data.UserPictureSource = BasePictureUri + jsonData.Data.UserPictureSource;
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
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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

		public async Task<UserToken> RegisterAsync(User user, string password, string cPassword)
		{
			if (password.Equals(cPassword))
			{
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
					{
						{
							"phone_number", user.PhoneNumber
						},
						{
							"clientType", user.UserType.ToString()
						},
						{
							"password", password
						},
						{
							"c_password", cPassword
						}
					});

					HttpResponseMessage response = await client.PostAsync(RegisterUri, encodedContent);
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
					return await Task.FromResult(new UserToken());
				}
			}

			return await Task.FromResult(new UserToken());
		}
		#endregion
	}
}
