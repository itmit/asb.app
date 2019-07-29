using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using itmit.asb.app.Services;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace itmit.asb.app.Models
{
	public class User
	{
		public UserToken UserToken 
		{
			get;
			private set;
		}

		public static async Task<User> GetUserByTokenAsync(UserToken token)
		{
			HttpResponseMessage response;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{token.TokenType} {token.Token}");

				response = await client.PostAsync(new Uri(Uri), null);
			}

			var jsonString = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(jsonString);

			if (response.IsSuccessStatusCode)
			{
				if (jsonString != null)
				{
					JsonDataResponse<User> jsonData = JsonConvert.DeserializeObject<JsonDataResponse<User>>(jsonString);
					jsonData.Data.UserToken = token;
					return await Task.FromResult(jsonData.Data);
				}
			}

			throw new AuthException($"Пользователь с таким токеном, не найден. Токен: {token.Token}");
		}

		public const string Uri = "http://asb.itmit-studio.ru/api/details";


		[JsonProperty("user_picture")]
		public string UserPictureSource
		{
			get;
			set;
		}

		public string Organization
		{
			get;
			set;
		}

		[JsonProperty("phone_number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		public string Node
		{
			get;
			set;
		}
		public string Email
		{
			get;
			set;
		}
	}
}
