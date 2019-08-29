using System;
using Newtonsoft.Json;
using Realms;

namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет токен доступа пользователя.
	/// </summary>
	public class UserToken : RealmObject
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает дату, до которой действует токен.
		/// </summary>
		[JsonProperty("expires_at")]
		public DateTimeOffset ExpiresAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает токен.
		/// </summary>
		[JsonProperty("access_token")]
		public string Token
		{
			get;
			set;
		} = string.Empty;

		/// <summary>
		/// Возвращает или устанавливает тип токена для авторизации.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType
		{
			get;
			set;
		} = "Bearer";
		#endregion
	}
}
