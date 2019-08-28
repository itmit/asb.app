using System;
using Newtonsoft.Json;

namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет токен доступа пользователя.
	/// </summary>
	public struct UserToken
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
		}

		/// <summary>
		/// Возвращает или устанавливает тип токена для авторизации.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType
		{
			get;
			set;
		}
		#endregion
	}
}
