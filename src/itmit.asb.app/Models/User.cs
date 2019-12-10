using System;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Newtonsoft.Json;
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

		[JsonProperty("on_duty")]
		public bool HasActiveBid
		{
			get;
			set;
		}

		[JsonProperty("bid_uuid")]
		public string BidGuid
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
		/// Возвращает или устанавливает паспорт пользователя.
		/// </summary>
        public string Passport 
        { 
            get;
            set; 
        }

        /// <summary>
		/// Возвращает или устанавливает ИНН пользователя.
		/// </summary>
        public string Inn 
        { 
            get;
            set;
        }

        /// <summary>
		/// Возвращает или устанавливает ОГРН пользователя.
		/// </summary>
        public string Ogrn 
        {
            get; 
            set; 
        }

        /// <summary>
		/// Возвращает или устанавливает директора пользователя.
		/// </summary>
        public string Director 
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

		[Ignored]
		public string ImageSource
		{
			get => UserPictureSource;
			set
			{
				UserPictureSource = value;
				CachedImage.InvalidateCache(UserPictureSource, CacheType.All, true);
			}
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
		/// Возвращает или устанавливает тип пользователя.
		/// </summary>
		[Ignored]
		public UserType UserType
		{
			get;
			set;
		}

        /// <summary>
		/// Возвращает или устанавливает тип пользователя.
		/// </summary>
		[JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает тип пользователя.
        /// </summary>
        [JsonProperty("is_active")]
		public bool IsActive
		{
			get;
			set;
		}

		[JsonProperty("active_from")]
		[Ignored]
		public DateTime? ActiveFromDateTime
		{
			get;
			set;
		}
		
		/// <summary>
		/// Возвращает или устанавливает дату начала активности.
		/// </summary>
		public DateTimeOffset ActiveFrom
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
