using System;
using Newtonsoft.Json;

namespace itmit.asb.app.Models
{
	public class Payment
	{
		[JsonProperty("is_active")]
		public bool IsActive
		{
			get;
			set;
		}

		[JsonProperty("active_from")]
		public DateTime ActiveFrom
		{
			get;
			set;
		}
	}
}
