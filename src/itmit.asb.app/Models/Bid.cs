using System;
using Newtonsoft.Json;

namespace itmit.asb.app.Models
{
	/// <summary>
	/// Представляет заявку клиента, которая формируется при нажатии на тревожную кнопку.
	/// </summary>
	public class Bid
	{
		#region Properties
		/// <summary>
		/// Возвращает или устанавливает клиент отправивший заявку.
		/// </summary>
		public User Client
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату создания заявки.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime CreatedAt
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает ид заявки.
		/// </summary>
		[JsonProperty("uid")]
		public Guid Guid
		{
			get;
			set;
		} = Guid.NewGuid();

		/// <summary>
		/// Возвращает или устанавливает координаты места, откуда была сделана заявка.
		/// </summary>
		public Location Location
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает статус заявки.
		/// </summary>
		public BidStatus Status
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает или устанавливает дату обновления заявки.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTime UpdatedAt
		{
			get;
			set;
		}
		#endregion
	}
}
