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
		private const string AuthUri = "http://lk.asb-security.ru/api/login";

		/// <summary>
		/// Задает адрес для регистрации.
		/// </summary>
		private const string RegisterUri = "http://lk.asb-security.ru/api/register";

		private const string ForgotPasswordUri = "http://lk.asb-security.ru/api/forgotPassword";

		private const string CheckCodeUri = "http://lk.asb-security.ru/api/checkCode";

		private const string ResetPasswordUri = "http://lk.asb-security.ru/api/resetPassword";

		private const string SetActivityFromUri = "http://lk.asb-security.ru/api/client/setActivityFrom";

		private const string CheckActivityStatusUri = "http://lk.asb-security.ru/api/client/chechClientActiveStatus";

		/// <summary>
		/// Задает адрес для получения картинок.
		/// </summary>
		private const string BasePictureUri = "http://lk.asb-security.ru/";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string DetailsUri = "http://lk.asb-security.ru/api/details";

		/// <summary>
		/// Задает ключ к api для авторизации.
		/// </summary>
		private const string SecretKey = "slYVEHsXme0pW4PoNzE9r2swGksXKb7VuKJF9DgO";

		/// <summary>
		/// Задает адрес для сохранения примечания.
		/// </summary>
		private const string SetNodeUri = "http://lk.asb-security.ru/api/client/note";

		/// <summary>
		/// Задает адрес для получения пользователя.
		/// </summary>
		private const string UploadImageUrl = "http://lk.asb-security.ru/api/client/changePhoto";

        /// <summary>
		/// Задает адрес для записи профиля пользователя.
		/// </summary>
        private const string SetProfile = "http://lk.asb-security.ru/api/client/edit";
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

        /// <summary>
        /// Устанавливает профиль пользователя.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        public async Task<bool> SetEditProfile(User user)
        {
            var type = user.UserToken.TokenType;
            var token = user.UserToken.Token;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{type} {token}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {
                        "name", user.Name
                    },
                    {
                        "passport", user.Passport
                    },
                    {
                        "email", user.Email
                    },
                    {
                        "organization", user.Organization
                    },
                    {
                        "INN", user.Inn
                    },
                    {
                        "OGRN", user.Ogrn
                    },
                    {
                        "director", user.Director
                    }
                });

                var response = await client.PostAsync(SetProfile, content);

                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(jsonString);

                return await Task.FromResult(response.IsSuccessStatusCode);
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

					if (jsonData.Data.ActiveFromDateTime != null)
					{
						jsonData.Data.ActiveFrom = (DateTimeOffset) jsonData.Data.ActiveFromDateTime;
					}

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

					var encodedContent = new Dictionary<string, string>
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
					};

					if (user.UserType == UserType.Entity)
					{
						encodedContent.Add("organization", user.Organization);
					}

					if (user.UserType == UserType.Individual)
					{
						encodedContent.Add("name", user.Name);
					}

					HttpResponseMessage response = await client.PostAsync(RegisterUri, new FormUrlEncodedContent(encodedContent));
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

		public async Task<bool> ForgotPassword(string phoneNumber)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"phone_number", phoneNumber
					}
				});

				HttpResponseMessage response = await client.PostAsync(ForgotPasswordUri, encodedContent);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		public async Task<bool> CheckCode(string phoneNumber, string code)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"phone_number", phoneNumber
					},
					{
						"secret_code", code
					}
				});

				HttpResponseMessage response = await client.PostAsync(CheckCodeUri, encodedContent);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		public async Task<bool> ResetPassword(string phoneNumber, string code, string password, string confirmPassword)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(SecretKey);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{
						"phone_number", phoneNumber
					},
					{
						"secret_code", code
					},
					{
						"new_password", password
					},
					{
						"new_password_confirmation", confirmPassword
					}
				});

				HttpResponseMessage response = await client.PostAsync(ResetPasswordUri, encodedContent);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		public async Task<bool> SetActivityFrom(UserToken token)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var encodedContent = new FormUrlEncodedContent(new Dictionary<string, string>());

				HttpResponseMessage response = await client.PostAsync(SetActivityFromUri, encodedContent);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				return await Task.FromResult(response.IsSuccessStatusCode);
			}
		}

		public async Task<User> SetActivityStatus(User user)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{user.UserToken.TokenType} {user.UserToken.Token}");

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


				var response = await client.PostAsync(CheckActivityStatusUri, null);
				var jsonString = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(jsonString);

				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
						//return await Task.FromResult(jsonData.Data);
					}
				}
				return await Task.FromResult(user);
			}
		}
		#endregion
	}
}
