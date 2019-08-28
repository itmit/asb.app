﻿using Newtonsoft.Json;
using Realms;

namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет пользователя приложения.
	/// </summary>
	public class User : RealmObject
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает email пользователя.
		/// </summary>
		public string Email
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает является ли пользователь охранником.
		/// </summary>
		[JsonProperty("is_guard")]
		public bool IsGuard
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает имя пользователя.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает примечания пользователя.
		/// </summary>
		public string Note
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает организацию пользователя.
		/// </summary>
		public string Organization
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает телефонный номер.
		/// </summary>
		[JsonProperty("phone_number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает аватар пользователя.
		/// </summary>
		[JsonProperty("user_picture")]
		public string UserPictureSource
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает токен пользователя.
		/// </summary>
		public UserToken UserToken
		{
			get;
			set;
		}
		#endregion
	}
}
