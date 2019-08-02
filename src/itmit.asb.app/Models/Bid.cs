using System;
using Newtonsoft.Json;

namespace itmit.asb.app.Models
{
	public class Bid
	{
		/// <summary>
		/// Возвращает или устанавливает ид заявки.
		/// </summary>
		[JsonProperty("uid")]
		public Guid Guid
		{
			get;
			set;
		} = Guid.NewGuid();

		public Location Location
		{
			get;
			set;
		}

		public User Client
		{
			get;
			set;
		}

		[JsonProperty("created_at")]
		public DateTime CreatedAt
		{
			get;
			set;
		}

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt 
		{
			get;
			set;
		}

		public BidStatus Status
		{
			get;
			set;
		}
	}
}
