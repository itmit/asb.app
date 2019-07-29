using System;

namespace itmit.asb.app.Models
{
	public class Bid
	{
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

		public DateTime CreatedAt
		{
			get;
			set;
		}

		public DateTime UpdatedAt 
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}
	}
}
